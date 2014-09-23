using BlockStyleColor;
using Expression.Blend.SampleData.SampleDataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8Theme.DataModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows8Theme
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserList : Page
    {
        MainPage rootPage = MainPage.Current;
        StoreData storeData = null;
        public UserList()
        {
            this.InitializeComponent();
            storeData = new StoreData();
            // set the source of the GridView to be the sample data
            ItemGridView2.ItemsSource = UsersDataModel.GetUsers();
        }

        private void Scenario2_ItemClickHandler(object sender, ItemClickEventArgs e)
        {
            Item _item = e.ClickedItem as Item;
           // rootPage.NotifyUser(String.Format("Clicked flavor of {0} is: {1}", _item.Category, _item.Title), NotifyType.StatusMessage);
        }

        void ItemGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            //ItemViewer iv = args.ItemContainer.ContentTemplateRoot as ItemViewer;

            //if (args.InRecycleQueue == true)
            //{
            //    iv.ClearData();
            //}
            //else if (args.Phase == 0)
            //{
            //    iv.ShowPlaceholder(args.Item as Item);

            //    // Register for async callback to visualize Title asynchronously
            //    args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            //}
            //else if (args.Phase == 1)
            //{
            //    iv.ShowTitle();
            //    args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            //}
            //else if (args.Phase == 2)
            //{
            //    iv.ShowCategory();
            //    iv.ShowImage();
            //}

            //// For imporved performance, set Handled to true since app is visualizing the data item
            //args.Handled = true;
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(ItemGridView_ContainerContentChanging);
                }
                return _delegate;
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;
    }
}
