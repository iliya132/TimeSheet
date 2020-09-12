
using CommonServiceLocator;
using TimeSheetApp.Model;
using TimeSheetApp.Model.Client;
using DryIoc;

namespace TimeSheetApp.ViewModel
{

    public class ViewModelLocator
    {
        Container currentContainer { get; set; }
        public ViewModelLocator()
        {
            currentContainer = new Container();
            currentContainer.Register<MainViewModel>();
            currentContainer.Register<IDataProvider, TimeSheetClient>();

        }

        public MainViewModel Main
        {
            get
            {
                return currentContainer.Resolve<MainViewModel>();
            }
        }
    }
}