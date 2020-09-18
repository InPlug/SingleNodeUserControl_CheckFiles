using System.Windows;
using Vishnu.Interchange;
using UserNodeControls;

namespace SingleNodeUserControl_CheckFiles
{
    /// <summary>
    /// Interaktionslogik für SingleNodeUserControl_CheckServer.xaml
    /// </summary>
    public partial class SingleNodeUserControl_CheckFiles : DynamicUserControlBase
    {
        /// <summary>
        /// Konstruktor - hängt einen EventHandler in ContentRendered.
        /// </summary>
        public SingleNodeUserControl_CheckFiles()
        {
            InitializeComponent();
            DynamicUserControl_ContentRendered
              += new RoutedEventHandler(content_Rendered);
        }

        /// <summary>
        /// Das ViewModell für das Result von FileChecker.
        /// </summary>
        public ResultViewModel UserResultViewModel { get; set; }

        private void ContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.UserResultViewModel = new ResultViewModel((IVishnuViewModel)this.DataContext);
                ((IVishnuViewModel)this.DataContext).UserDataContext = this.UserResultViewModel;
            }
        }

        private void content_Rendered(object sender, RoutedEventArgs e)
        {
            if (this.UserResultViewModel != null)
            {
                this.UserResultViewModel.HandleResultPropertyChanged();
            }
        }

        //private delegate void NoArgDelegate();

        //public static void Refresh(DependencyObject obj)
        //{
        //  obj.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, (NoArgDelegate)delegate	{	});
        //}
    }
}
