
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

using TimeSheetApp.Model;
using TimeSheetApp.Model.Client;

namespace TimeSheetApp.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            
            SimpleIoc.Default.Register<Model.IDataProvider, TimeSheetClient>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}