using System;
using System.IO;
using EnvDTE;

namespace NModelsGenerator.Common
{
    public static class ProjectHelpers
    {
        public static string GetRootNamespace(this Project project)
        {
            if (project == null)
                return null;

            string ns = project.Name ?? string.Empty;

            try
            {
                var prop = project.Properties.Item("RootNamespace");

                if (prop != null && prop.Value != null && !string.IsNullOrEmpty(prop.Value.ToString()))
                    ns = prop.Value.ToString();
            }
            catch { /* Project doesn't have a root namespace */ }

            return CleanNameSpace(ns, stripPeriods: false);
        }

        public static string CleanNameSpace(string ns, bool stripPeriods = true)
        {
            if (stripPeriods)
            {
                ns = ns.Replace(".", "");
            }

            ns = ns.Replace(" ", "")
                .Replace("-", "")
                .Replace("\\", ".");

            return ns;
        }

        public static string GetRootFolder(this Project project)
        {
            if (string.IsNullOrEmpty(project?.FullName))
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item("FullPath").Value as string;
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item("ProjectDirectory").Value as string;
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item("ProjectPath").Value as string;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
                return File.Exists(project.FullName) ? Path.GetDirectoryName(project.FullName) : null;

            if (Directory.Exists(fullPath))
                return fullPath;

            if (File.Exists(fullPath))
                return Path.GetDirectoryName(fullPath);

            return null;
        }

        public static ProjectItem AddFileToProject(this Project project, string file, string itemType = null)
        {
            //if (project.IsKind(ProjectTypes.Aspnet5))
            //    return _dte.Solution.FindProjectItem(file);

            ProjectItem item = project.ProjectItems.AddFromFile(file);
            item.SetItemType(itemType);
            return item;
        }

        public static void SetItemType(this ProjectItem item, string itemType)
        {
            try
            {
                if (item?.ContainingProject == null)
                    return;

                if (string.IsNullOrEmpty(itemType)
                    || item.ContainingProject.IsKind(ProjectTypes.WebsiteProject)
                    || item.ContainingProject.IsKind(ProjectTypes.UniversalApp))
                    return;

                item.Properties.Item("ItemType").Value = itemType;
            }
            catch (Exception)
            {
                //Logger.Log(ex);
            }
        }

        public static bool IsKind(this Project project, string kindGuid)
        {
            return project.Kind.Equals(kindGuid, StringComparison.OrdinalIgnoreCase);
        }

        public static Project GetActiveProject(this DTE dte)
        {
            try
            {
                var activeSolutionProjects = dte.ActiveSolutionProjects as Array;

                if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
                    return activeSolutionProjects.GetValue(0) as Project;

                var doc = dte.ActiveDocument;

                if (!string.IsNullOrEmpty(doc?.FullName))
                {
                    var item = dte.Solution?.FindProjectItem(doc.FullName);

                    if (item != null)
                        return item.ContainingProject;
                }
            }
            catch (Exception ex)
            {
                var mes = "Error getting the active project" + ex;
                throw;
            }

            return null;
        }


        public static ProjectItem FindProjectItemInProject(this Project project, string name)
        {


            var item = project.DTE.Solution.FindProjectItem(name);
            if (item != null)
            {
                var n = item.Name;
            }
            return item;
        }

    }

    public static class ProjectTypes
    {
        public const string Aspnet5 = "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}";
        public const string WebsiteProject = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
        public const string UniversalApp = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
        public const string NodeJs = "{9092AA53-FB77-4645-B42D-1CCCA6BD08BD}";
        public const string PhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        public const string PhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
    }
}