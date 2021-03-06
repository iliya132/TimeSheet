
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

using TimeSheetApp.Model;
using TimeSheetApp.Model.Client;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            
            SimpleIoc.Default.Register<IIdentityProvider, IdentityClient>();
            SimpleIoc.Default.Register<IDataProvider, TimeSheetClient>();
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