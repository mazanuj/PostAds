using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Motorcycle.Config;

namespace Motorcycle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
// ReSharper disable once RedundantExtendsListEntry
// ReSharper disable once UnusedMember.Global
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_Click(object sender, MouseButtonEventArgs e)
        {
            var tab = (sender as Label);
            if (tab == null) return;

            switch (tab.Name)
            {
                case "LabelSettings":
                case "LabelCommon":
                    TextBoxCaptchaDomain.Text = GetSettings.GetCaptcha("domain");
                    TextBoxCaptchaKey.Text = GetSettings.GetCaptcha("key");
                    TextBoxCaptchaDomain.Background = Brushes.DarkGreen;
                    TextBoxCaptchaKey.Background = Brushes.DarkGreen;
                    break;
                case "LabelAddMan":
                    ListViewManufactures.Items.Clear();
                    foreach (var value in GetSettings.GetManufactures)
                        ListViewManufactures.Items.Add(value);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            switch (button.Name)
            {
                case "ButtonStart":
                    //TODO Create data for posting
                    break;
                case "ButtonClearLog":
                    ListBoxLog.Items.Clear();
                    break;
                case "ButtonCaptcha":
                    switch (button.Content as string)
                    {
                        case "Изменить":
                            ButtonCaptcha.Content = "Сохранить";

                            TextBoxCaptchaDomain.IsReadOnly = false;
                            TextBoxCaptchaKey.IsReadOnly = false;

                            TextBoxCaptchaDomain.Background = Brushes.AliceBlue;
                            TextBoxCaptchaKey.Background = Brushes.AliceBlue;

                            TextBoxCaptchaDomain.Foreground = Brushes.Black;
                            TextBoxCaptchaKey.Foreground = Brushes.Black;
                            break;

                        case "Сохранить":
                            ButtonCaptcha.Content = "Изменить";

                            TextBoxCaptchaDomain.IsReadOnly = true;
                            TextBoxCaptchaKey.IsReadOnly = true;

                            TextBoxCaptchaDomain.Background = Brushes.DarkGreen;
                            TextBoxCaptchaKey.Background = Brushes.DarkGreen;

                            TextBoxCaptchaDomain.Foreground = Brushes.Yellow;
                            TextBoxCaptchaKey.Foreground = Brushes.Yellow;

                            SetSettings.SetCaptcha("domain", TextBoxCaptchaDomain.Text);
                            SetSettings.SetCaptcha("m", TextBoxCaptchaKey.Text);
                            break;
                    }
                    break;
            }
        }

        private void ReloadModels(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            ListViewModels.Items.Clear();
            foreach (var value in GetSettings.GetModels(id.ToLower()))
                ListViewModels.Items.Add(new AddManufacture(value.Key, value.Value));
            ListViewModels.SelectedIndex = 0;
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            dynamic t = e.AddedItems[0];
            ReloadModels(t.Id);
        }

        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var fe = e.Source as FrameworkElement;
            var listView = sender as ListView;
            if (listView == null || fe == null) return;

            var contextMenu = new ContextMenu();
            var item = new MenuItem {Header = "Добавить"};
            item.Click += ContextMenu_Click;
            contextMenu.Items.Add(item);

            if (listView.SelectedItem != null)
            {
                var item1 = new MenuItem {Header = "Изменить"};
                var item2 = new MenuItem {Header = "Удалить"};

                item1.Click += ContextMenu_Click;
                item2.Click += ContextMenu_Click;

                contextMenu.Items.Add(item1);
                contextMenu.Items.Add(item2);
            }
            fe.ContextMenu = contextMenu;
            fe.ContextMenu.Name = listView.Name == "ListViewManufactures"
                ? "ContextMenuManufactures"
                : "ContextMenuModels";
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi == null) return;

            var contextMenu = mi.Parent as ContextMenu;
            if (contextMenu == null) return;

            switch (contextMenu.Name)
            {
                case "ContextMenuManufactures":
                    var db = new Confirmation
                    {
                        TextBlockName = {Text = "Завод"},
                        ButtonConfirm = {Content = mi.Header as string}
                    };

                    switch (mi.Header as string)
                    {
                        case "Добавить":
                            var result = db.ShowDialog() ?? false;
                            if (!result) return;

                            if (db.TextBoxName.Text == string.Empty || 
                                db.TextBoxM.Text == string.Empty ||
                                db.TextBoxP.Text == string.Empty ||
                                db.TextBoxU.Text == string.Empty) return;

                            if (SetSettings.SetManufacture(db.TextBoxName.Text, db.TextBoxM.Text, db.TextBoxP.Text, db.TextBoxU.Text))
                            {
                                TabControl_Click(LabelAddMan, null);
                                ListViewModels.Items.Clear();
                            }
                            break;

                        case "Изменить":
                            var id = db.TextBoxName.Text = (ListViewManufactures.SelectedValue as dynamic).Id;
                            db.TextBoxM.Text = (ListViewManufactures.SelectedValue as dynamic).M;
                            db.TextBoxP.Text = (ListViewManufactures.SelectedValue as dynamic).P;
                            db.TextBoxU.Text = (ListViewManufactures.SelectedValue as dynamic).U;

                            result = db.ShowDialog() ?? false;
                            if (!result) return;

                            if (db.TextBoxName.Text == string.Empty ||
                                db.TextBoxM.Text == string.Empty ||
                                db.TextBoxP.Text == string.Empty ||
                                db.TextBoxU.Text == string.Empty) return;

                            if (SetSettings.ChangeManufacture(id, db.TextBoxName.Text, db.TextBoxM.Text, db.TextBoxP.Text, db.TextBoxU.Text))
                                TabControl_Click(LabelAddMan, null);
                            break;

                        case "Удалить":
                            db.TextBoxName.Text = (ListViewManufactures.SelectedValue as dynamic).Id;
                            db.TextBoxM.Text = (ListViewManufactures.SelectedValue as dynamic).M;
                            db.TextBoxP.Text = (ListViewManufactures.SelectedValue as dynamic).P;
                            db.TextBoxU.Text = (ListViewManufactures.SelectedValue as dynamic).U;

                            db.TextBoxName.IsEnabled = db.TextBoxM.IsEnabled = db.TextBoxP.IsEnabled = db.TextBoxU.IsEnabled = false;


                            result = db.ShowDialog() ?? false;
                            if (!result) return;

                            if (SetSettings.DeleteManufacture(db.TextBoxName.Text))
                            {
                                TabControl_Click(LabelAddMan, null);
                                ListViewModels.Items.Clear();
                            }
                            break;
                    }
                    break;

                case "ContextMenuModels":
                    var bd = new Confirmation(75)
                    {
                        TextBlockName = {Text = "Модель"},
                        ButtonConfirm = {Content = mi.Header as string},
                        TextBoxP = {Visibility = Visibility.Hidden},
                        TextBoxU = {Visibility = Visibility.Hidden},
                        TextBlockP = {Visibility = Visibility.Hidden},
                        TextBlockU = {Visibility = Visibility.Hidden}
                    };

                    switch (mi.Header as string)
                    {
                        case "Добавить":
                            var result = bd.ShowDialog() ?? false;
                            if (!result) return;

                            if (bd.TextBoxName.Text == string.Empty || bd.TextBoxM.Text == string.Empty) return;

                            var id = (ListViewManufactures.SelectedValue as dynamic).Id;

                            if (SetSettings.SetModel(id, bd.TextBoxName.Text, bd.TextBoxM.Text))
                                ReloadModels(id);
                            break;

                        case "Изменить":
                            id = bd.TextBoxName.Text = (ListViewModels.SelectedValue as dynamic).Id;
                            bd.TextBoxM.Text = (ListViewModels.SelectedValue as dynamic).M;

                            result = bd.ShowDialog() ?? false;
                            if (!result) return;
                            if (bd.TextBoxName.Text == string.Empty || bd.TextBoxM.Text == string.Empty) return;

                            if (SetSettings.ChangeModel((ListViewManufactures.SelectedValue as dynamic).Id, id, bd.TextBoxName.Text, bd.TextBoxM.Text))
                                ReloadModels((ListViewManufactures.SelectedValue as dynamic).Id);
                            break;

                        case "Удалить":                            
                            bd.TextBoxName.Text = (ListViewModels.SelectedValue as dynamic).Id;
                            bd.TextBoxM.Text = (ListViewModels.SelectedValue as dynamic).M;
                            
                            bd.TextBoxName.IsEnabled = false;
                            bd.TextBoxM.IsEnabled = false;

                            result = bd.ShowDialog() ?? false;
                            if (!result) return;

                            if (SetSettings.DeleteModel((ListViewManufactures.SelectedValue as dynamic).Id, bd.TextBoxName.Text))
                                ReloadModels((ListViewManufactures.SelectedValue as dynamic).Id);
                            break;
                    }
                    break;
            }
        }
    }
}