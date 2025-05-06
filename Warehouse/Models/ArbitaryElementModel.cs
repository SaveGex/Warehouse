using Warehouse.DataBase.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace Warehouse.Models;

class ArbitaryElementModel : ObservableObject
{
    private BaseElement _element;
    public BaseElement Element
    {
        get => _element;
        set
        {
            _element = value;
            OnPropertyChanged(nameof(Element));
        }
    }

    public ArbitaryElementModel(BaseElement some_element)
    {
        _element = some_element;
        Element = some_element;
        
    }

    public ArbitaryElementModel()
    {
        //Plug for empty element to which refered other elements
        Element = new BaseElement();
    }


    public void BindingProperties(BaseElement some_element)
    {
        Element = some_element;
        _element = some_element;
        OnPropertyChanged(nameof(Element));
    }

}
