using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using NModelsGenerator.Common;
using NModelsGenerator.Core;
using Constants = NModelsGenerator.Common.Constants;

namespace NModelsGenerator.Vsix
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateModelsCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("309063dc-3ad2-4989-9c84-b81fdbda6b37");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        private static DTE _dte;
        private INModelsGenerator _generator;
        private readonly Project _project;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateModelsCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private GenerateModelsCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            _dte = (DTE)this.ServiceProvider.GetService(typeof(DTE));
            _project = _dte.GetActiveProject();
            _generator = new Generator(new DirectoryInfo(_project.GetRootFolder()));


            if (this.ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateModelsCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this._package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new GenerateModelsCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            bool hasConfigFile = ProjectHasConfigFile();

            if (!hasConfigFile)
            {
                var mes = MessageBox.Show(
                    $"NModels Config Json file by the name {Constants.ConfigFileName} must be added to the project in order to generate Models Classes. Do you want to add a json config file ?",
                    "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                if (mes == DialogResult.Yes)
                {
                    _generator.Init();
                    var configFilePath = _generator.GetConfigFilePath();
                    var projectItem = _project.ProjectItems.AddFromFile(configFilePath);
                    OpenSyncToDocument(configFilePath);
                }

            }
            else
            {
                var config = MiscUtils.GetConfig(_project.GetRootFolder());
                //set up the generator
                _generator.Run(config);
            }

        }



        private void OpenSyncToDocument(string file)
        {
            VsShellUtilities.OpenDocument(ServiceProvider, file);

            _dte.ExecuteCommand("SolutionExplorer.SyncWithActiveDocument");
            _dte.ActiveDocument.Activate();
            _dte.ExecuteCommand("Edit.FormatDocument");
        }

        private bool ProjectHasConfigFile()
        {
            return _project.FindProjectItemInProject(Constants.ConfigFileName) != null;
        }

        private static string GetConfigFilePath(Project project)
        {
            var configFileFullPath = "";
            var projectItems = project.ProjectItems;
            short itemCounter = 0;
            foreach (ProjectItem item in projectItems)
            {
                var filePath = item.FileNames[itemCounter];
                var fileName = Path.GetFileName(filePath);
                if (fileName != null && fileName.Equals(Constants.ConfigFileName, StringComparison.OrdinalIgnoreCase))
                {
                    configFileFullPath = filePath;
                    break;
                };
                itemCounter++;
            }
            return configFileFullPath;
        }

        /// <summary>
        /// event handler that will be called before the menu command is displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            if (sender is OleMenuCommand myCommand)
            {
                var nModelsConfigFile = _project.FindProjectItemInProject(Constants.ConfigFileName);
                myCommand.Text = nModelsConfigFile == null ? "Add NModels Config File" : "Generate Models";
            }
        }
    }
}
