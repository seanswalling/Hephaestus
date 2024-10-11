using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Hephaestus.Avalonia.Models;
using Hephaestus.Core.Domain;

namespace Hephaestus.Avalonia.ViewModels
{
    public partial class ProjectDataGridViewModel : ViewModelBase
    {
        private readonly RepositoryProviderUIAdapter _adapter;
        public ProjectViewModel[] Projects { get; set; }

        [ObservableProperty]
        public ObservableCollection<ProjectViewModel> _dataTableContents;

        [ObservableProperty]
        private ProjectViewModel? _selectedProject;

        [ObservableProperty]
        private ProjectFormat? _selectedProjectFormat;

        [ObservableProperty]
        private OutputType? _selectedOutputType;

        [ObservableProperty]
        private Framework? _selectedFramework;

        [ObservableProperty]
        private string? _filterString;

        [ObservableProperty]
        public int _count;

        [ObservableProperty]
        public string _someTestText;

        public static ProjectFormat[] ProjectFormats => Enum.GetValues(typeof(ProjectFormat)).Cast<ProjectFormat>().ToArray();
        public static OutputType[] OutputTypes => Enum.GetValues(typeof(OutputType)).Cast<OutputType>().ToArray();
        public static Framework[] Frameworks => Enum.GetValues(typeof(Framework)).Cast<Framework>().ToArray();

        public ProjectDataGridViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            Projects = _adapter.GetProjects();
            DataTableContents = new ObservableCollection<ProjectViewModel>(_adapter.GetProjects());
            SelectedProject = Projects.First();
            FilterString = string.Empty;
            SelectedProjectFormat = ProjectFormat.Unknown;
            SelectedOutputType = OutputType.Unknown;
            SelectedFramework = Framework.Unknown;
            SomeTestText = string.Empty;
            Filter();
        }

        private void Filter()
        {
            var name = (FilterString ?? string.Empty).ToLowerInvariant();
            var format = SelectedProjectFormat ?? ProjectFormat.Unknown;
            var output = SelectedOutputType ?? OutputType.Unknown;
            var framework = SelectedFramework ?? Framework.Unknown;
            var filteredRows = Projects
                .Where(Filter(name))
                .Where(Filter(format))
                .Where(Filter(output))
                .Where(Filter(framework))
                .ToArray();
            DataTableContents = new ObservableCollection<ProjectViewModel>(filteredRows.OrderByDescending(x => x.Usages.Length));
            Count = DataTableContents?.Count ?? 0;
        }

        private static Func<ProjectViewModel, bool> Filter(string filterStr) => (vm) =>
        {
            if (string.IsNullOrWhiteSpace(filterStr)) return true;
            if (vm.Name.ToLower().Contains(filterStr)) return true;
            return false;
        };

        private static Func<ProjectViewModel, bool> Filter(ProjectFormat format) => (vm) =>
        {
            if (format == ProjectFormat.Unknown) return true;
            if (vm.Format == format) return true;
            return false;
        };

        private static Func<ProjectViewModel, bool> Filter(OutputType output) => (vm) =>
        {
            if (output == OutputType.Unknown) return true;
            if (vm.OutputType == output) return true;
            return false;
        };

        private static Func<ProjectViewModel, bool> Filter(Framework framework) => (vm) =>
        {
            if (framework == Framework.Unknown) return true;
            if (vm.Framework == framework) return true;
            return false;
        };
    }
}