namespace OdessaGUIProject.Workers
{
    internal abstract class AbstractExportWorker
    {
        internal string ProjectFileLocation;

        internal abstract string AppName { get; }

        internal abstract bool ExportHighlightsToFile(string outputPath);

        internal abstract string GetPathToApp();

        internal abstract string GetProjectFileExtension();

        internal abstract string GetProjectFilename();

        internal abstract string GetSaveDialogFilter();

        internal abstract string LaunchAppMessage();

        /// <summary>
        /// Whether we can simply run the project file and have the app load.
        /// If this is false, we'll have to find the app exe and hopefully pass the project file as a command line
        /// </summary>
        /// <returns></returns>
        internal abstract bool SupportsLaunchingByExtension();
    }
}