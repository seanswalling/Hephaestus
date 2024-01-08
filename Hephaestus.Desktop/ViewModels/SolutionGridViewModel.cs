using System;
using System.Collections.ObjectModel;
using System.Linq;
using Hephaestus.Desktop.Models;

namespace Hephaestus.Desktop.ViewModels
{
    public class SolutionGridViewModel : ViewModelBase
    {
        private readonly RepositoryProviderUIAdapter _adapter;
        private SolutionViewModel? _selectedSolution;
        public SolutionViewModel[] Solutions { get; set; }
        public ObservableCollection<SolutionViewModel> DataTableContents { get; set; } = null!;

        private string _filterString = null!;
        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                Filter(value.ToLowerInvariant());
            }
        }


        public SolutionViewModel? SelectedSolution
        {
            get => _selectedSolution;
            set
            {
                _selectedSolution = value;
                OnPropertyChanged(nameof(SelectedSolution));
            }
        }

        public SolutionGridViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            Solutions = _adapter.GetSolutions();
            _selectedSolution = Solutions.First();
            FilterString = string.Empty;
            Filter(FilterString);
        }

        private void Filter(string filter)
        {
            var filteredRows = Solutions.Where(FilterPredicate(filter)).ToArray();
            DataTableContents = new ObservableCollection<SolutionViewModel>(filteredRows.OrderBy(x => x.Path));
            OnPropertyChanged(nameof(DataTableContents));
        }

        private static Func<SolutionViewModel, bool> FilterPredicate(string filterStr) => (vm) =>
        {
            if (string.IsNullOrWhiteSpace(filterStr)) return true;
            if (vm.Name.ToLower().Contains(filterStr)) return true;
            return false;
        };
    }
}
