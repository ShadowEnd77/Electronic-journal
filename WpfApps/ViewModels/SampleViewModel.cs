using Prism.Commands;
using Prism.Mvvm;
using WpfCustomControlLibrary.Models;

public class SampleViewModel : BindableBase
{
    private SampleModel _model;

    public string Name
    {
        get => _model.Name;
        //set => SetProperty(ref _model.Name, value);
    }

    public SampleViewModel(SampleModel model)
    {
        _model = model;
    }
}