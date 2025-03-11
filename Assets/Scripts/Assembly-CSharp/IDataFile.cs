public abstract class IDataFile
{
	public abstract void SetInt(string key, int value);

	public abstract int GetInt(string key, int defaultValue = 0);

	public abstract void SetFloat(string key, float value);

	public abstract float GetFloat(string key, float defaultValue = 0f);

	public abstract void SetString(string key, string value);

	public abstract string GetString(string key, string defaultValue = "");

	public abstract bool KeyExists(string key);
}
