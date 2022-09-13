using Vishnu.ViewModel;
using UserNodeControls;
using Vishnu.Interchange;

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
            // DynamicUserControl_ContentRendered += new RoutedEventHandler(content_Rendered);
        }

        /// <summary>
        /// Konkrete Überschreibung von GetUserResultViewModel, returnt ein spezifisches ResultViewModel.
        /// </summary>
        /// <param name="vishnuViewModel">Ein spezifisches ResultViewModel.</param>
        /// <returns></returns>
        protected override DynamicUserControlViewModelBase GetUserResultViewModel(IVishnuViewModel vishnuViewModel)
        {
            return new ResultViewModel((IVishnuViewModel)this.DataContext);
        }
    }
}
