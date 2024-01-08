using System;
using System.Collections.ObjectModel;
using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Desktop.Models;

namespace Hephaestus.Desktop.ViewModels
{
    public class ProjectGridViewModel : ViewModelBase
    {
        private readonly RepositoryProviderUIAdapter _adapter;
        public ProjectViewModel[] Projects { get; set; }
        public ObservableCollection<ProjectViewModel> DataTableContents { get; set; } = null!;
        public int Count => DataTableContents?.Count ?? 0;

        private string? _filterString;
        public string FilterString
        {
            get => _filterString ?? string.Empty;
            set
            {
                _filterString = value;
                Filter();
            }
        }

        private ProjectViewModel? _selectedProject;
        public ProjectViewModel? SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private ProjectFormat? _selectedProjectFormat;
        public ProjectFormat[] ProjectFormats => Enum.GetValues(typeof(ProjectFormat)).Cast<ProjectFormat>().ToArray();
        public ProjectFormat? SelectedProjectFormat
        {
            get => _selectedProjectFormat;
            set
            {
                _selectedProjectFormat = value;
                Filter();
            }
        }

        private OutputType? _selectedOutputType;
        public OutputType[] OutputTypes => Enum.GetValues(typeof(OutputType)).Cast<OutputType>().ToArray();
        public OutputType? SelectedOutputType
        {
            get => _selectedOutputType;
            set
            {
                _selectedOutputType = value;
                Filter();
            }
        }

        private Framework? _selectedFramework;

        public Framework[] Frameworks => Enum.GetValues(typeof(Framework)).Cast<Framework>().ToArray();
        public Framework? SelectedFramework
        {
            get => _selectedFramework;
            set
            {
                _selectedFramework = value;
                Filter();
            }
        }

        public ProjectGridViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            Projects = _adapter.GetProjects();
            _selectedProject = Projects.First();
            FilterString = string.Empty;
            SelectedProjectFormat = ProjectFormat.Unknown;
            SelectedOutputType = OutputType.Unknown;
            SelectedFramework = Framework.Unknown;
            Filter();
        }

        private void Filter()
        {
            var name = (_filterString ?? string.Empty).ToLowerInvariant();
            var format = _selectedProjectFormat ?? ProjectFormat.Unknown;
            var output = _selectedOutputType ?? OutputType.Unknown;
            var framework = _selectedFramework ?? Framework.Unknown;
            var filteredRows = Projects
                .Where(Filter(name))
                .Where(Filter(format))
                .Where(Filter(output))
                .Where(Filter(framework))
                .ToArray();
            DataTableContents = new ObservableCollection<ProjectViewModel>(filteredRows.OrderByDescending(x => x.Usages.Length));
            OnPropertyChanged(nameof(DataTableContents));
            OnPropertyChanged(nameof(Count));
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