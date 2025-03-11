public static class ConvertMissionType
{
	public static E_MissionType Convert(E_MissionTypeEditor missionType)
	{
		return (E_MissionType)missionType;
	}

	public static E_MissionTypeEditor Convert(E_MissionType missionType)
	{
		return (E_MissionTypeEditor)missionType;
	}
}
