using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Hephaestus.Avalonia.ViewModels
{
    public partial class FileContentViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _content;

        //public string Content
        //{
        //    get => _content;
        //    set
        //    {
        //        _content = value;
        //        Document = new TextDocument(value);
        //    }
        //}

        [ObservableProperty]
        private TextDocument _textDocument;
        //public TextDocument Document
        //{
        //    get => _textDocument;
        //    set
        //    {
        //        _textDocument = value;
        //        OnPropertyChanged(nameof(Document));
        //    }
        //}

        public FileContentViewModel()
        {
            Content = "Hello this is a File Content Window!";
            TextDocument = new TextDocument(_content);
            //OnPropertyChanged(nameof(Content));
            //OnPropertyChanged(nameof(Document));
        }
    }
}
