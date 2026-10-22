# Strict readers for supplied Unity 4.5 v13 and Unity 2017 v16 layouts.
from pathlib import Path
import argparse, json, math, re, struct

U32=lambda b,o:struct.unpack_from('<I',b,o)[0]
I32=lambda b,o:struct.unpack_from('<i',b,o)[0]
F32=lambda b,o:struct.unpack_from('<f',b,o)[0]
VEC=lambda b,o:struct.unpack_from('<3f',b,o)

def legacy_tiles(path):
    match=re.search(r'^\s*m_MeshData:\s*([a-fA-F0-9]+)',path.read_text('utf-8-sig'),re.M)
    if not match:raise ValueError('Legacy asset must be Unity text serialized with m_MeshData')
    data=bytes.fromhex(match.group(1))
    if data[:4]!=b'TESM':raise ValueError('Expected TESM container')
    count=U32(data,8)
    offset=40
    for i in range(count):
        _,length=struct.unpack_from('<II',data,offset)
        tile=data[offset+8:offset+8+length]
        if len(tile)!=length or tile[:4]!=b'VAND' or U32(tile,4)!=13:raise ValueError('Invalid legacy tile')
        yield i,tile
        offset+=8+length
    if offset!=len(data):raise ValueError('Trailing TESM bytes')

def convert_tile(old):
    version,x,z,layer,user,pc,vc,links,dm,dv,dt,bv,offmesh,offbase=struct.unpack_from('<IiiiIIIIIIIIII',old,4)
    if version!=13 or pc!=offbase+offmesh or dm!=offbase:
        raise ValueError('Unsupported legacy tile layout/counts')
    offset=104
    vertices=old[offset:offset+12*vc];offset+=12*vc
    polys=[old[offset+36*i:offset+36*(i+1)] for i in range(pc)];offset+=36*pc
    offset+=12*links # Detour dynamic link records are rebuilt by Unity
    detailmesh=old[offset:offset+12*dm];offset+=12*dm
    detailverts=old[offset:offset+12*dv];offset+=12*dv
    detailtri=old[offset:offset+8*dt];offset+=8*dt
    bvtree=old[offset:offset+16*bv];offset+=16*bv
    offset+=40*offmesh
    if offset!=len(old):raise ValueError('Unsupported sections in old tile: %d vs %d'%(offset,len(old)))
    for i,p in enumerate(polys):
        if len(p)!=36:raise ValueError('Truncated polygon')
        nv=p[32]
        area_type=p[33]
        if i<dm and (nv<3 or nv>6 or area_type>>6!=0):raise ValueError('Unexpected surface polygon')
        if i>=dm and (nv!=2 or area_type>>6!=1):raise ValueError('Unexpected off-mesh polygon')
        if i<dm:
            ids=struct.unpack_from('<6H',p,4)
            if any(v>=vc for v in ids[:nv]):raise ValueError('Bad polygon vertex index')
            for neighbor in struct.unpack_from('<6H',p,16)[:nv]:
                if neighbor and not (neighbor & 0x8000) and neighbor>dm:
                    raise ValueError('Ground polygon refers to removed off-mesh polygon')
    # Header v16 omits user ID, runtime link allocations, walkable dimensions,
    # and embedded off-mesh connection fields. bmin/bmax/bvQuantFactor retained.
    header=struct.pack('<4sIiiiIIIIII',b'VAND',16,x,z,layer,dm,vc,dm,dv,dt,bv)+old[76:104]
    if len(header)!=72:raise AssertionError('Incorrect modern tile header size')
    # v16 polygon is v13 polygon without firstLink (4-byte runtime link field).
    # The four final bytes preserve legacy traversal flags, vertex count & area.
    ground=b''.join(p[4:] for p in polys[:dm])
    new=header+vertices+ground+detailmesh+detailverts+detailtri+bvtree
    expect=72+12*vc+32*dm+12*dm+12*dv+8*dt+16*bv
    if len(new)!=expect:raise AssertionError('Modern tile section sizes disagree')
    return new,{'x':x,'z':z,'layer':layer,'polygons':dm,'vertices':vc,'detail_triangles':dt,'removed_offmesh_polygons':offmesh,'old_size':len(old),'new_size':len(new),'bounds_min':VEC(old,76),'bounds_max':VEC(old,88)}

def read_2017_template(path):
    b=path.read_bytes()
    if len(b)<4240:raise ValueError('2017 reference asset truncated')
    metadata_size,file_size,version,data_offset=struct.unpack_from('>4I',b,0)
    if version!=17 or file_size!=len(b) or b'2017.4.40f1\0' not in b[:64]:raise ValueError('Expected Unity 2017.4.40f1 binary .asset')
    if b.find(b'NavMeshData')<0 or b.find(b'm_NavMeshTiles')<0:raise ValueError('Expected NavMeshData type tree')
    if b.find(b'VAND')<0:raise ValueError('Reference contains no populated VAND tile')
    # Metadata serialized-object record found dynamically: single 238-class object.
    # The template object size is stored 28 bytes before metadata end (v17 layout).
    signature=struct.pack('<I',len(b)-data_offset)
    object_size_at=b.find(signature,metadata_size-128,data_offset)
    if object_size_at<0:raise ValueError('Cannot locate serialized object size record')
    obj=b[data_offset:]
    # Two 12-byte PPtr, 4-byte int m_Name length, padded string, tile count.
    name_len=U32(obj,28)
    tile_count_pos=(32+name_len+3)&~3
    if name_len>256 or tile_count_pos+4>=len(obj):raise ValueError('Unexpected object string layout')
    count=U32(obj,tile_count_pos)
    off=tile_count_pos+4
    for i in range(count):
        n=U32(obj,off)
        off+=4
        tile=obj[off:off+n]
        if len(tile)!=n or tile[:4]!=b'VAND' or U32(tile,4)!=16:raise ValueError('Malformed 2017 template tile')
        off+=n+16 # Hash128
    if count<1 or len(obj)-off!=124:raise ValueError('Expected the 2017.4 reference tail (124 bytes)')
    return b,obj[:tile_count_pos],obj[off:],metadata_size,data_offset,object_size_at
