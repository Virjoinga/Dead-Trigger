namespace Madfinger.Plugins
{
	public interface IBuildProcessor
	{
		bool PreProcess(EBuildPlatform platform);

		bool PostProcess(EBuildPlatform platform, string buildPath);
	}
}
