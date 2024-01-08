using System;
using ICSharpCode.AvalonEdit.Document;

namespace Hephaestus.Desktop.ViewModels
{
    public class PreviewViewModel : ViewModelBase
    {
        public FileContentViewModel File { get; set; }
        public ReferencesViewModel References { get; set; }
        public UsagesViewModel Usages { get; set; }

        public PreviewViewModel()
        {
            File = new FileContentViewModel();
            References = new ReferencesViewModel();
            Usages = new UsagesViewModel();
        }
    }

    public class FileContentViewModel : ViewModelBase
    {
        private string _content;

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                Document = new TextDocument(value);
            }
        }

        private TextDocument _textDocument;
        public TextDocument Document
        {
            get => _textDocument;
            set
            {
                _textDocument = value;
                OnPropertyChanged(nameof(Document));
            }
        }

        public FileContentViewModel()
        {
            _content = "Hello this is a File Content Window!";
            _textDocument = new TextDocument(_content);
            //OnPropertyChanged(nameof(Content));
            OnPropertyChanged(nameof(Document));
        }
    }

    public class UsagesViewModel : ViewModelBase
    {
        private ProjectViewModel[] _projects;

        public ProjectViewModel[] Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public UsagesViewModel()
        {
            _projects = Array.Empty<ProjectViewModel>();
            OnPropertyChanged(nameof(Projects));
        }
    }

    public class ReferencesViewModel : ViewModelBase
    {
        private ProjectViewModel[] _projects;

        public ProjectViewModel[] Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public ReferencesViewModel()
        {
            _projects = Array.Empty<ProjectViewModel>();
            OnPropertyChanged(nameof(Projects));
        }
    }
}
