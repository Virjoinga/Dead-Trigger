public class BladesChopperDamage : MeleeDamageData
{
	public E_BodyPart ChoppedOffPart { get; private set; }

	public float ReducedHealthCoef { get; set; }

	public BladesChopperDamage(E_BodyPart Part)
	{
		ChoppedOffPart = Part;
		ReducedHealthCoef = 1f;
	}
}
