using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace new_pr_2._24
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatePicker dateP = new DatePicker();
        List<Note> noteList = new List<Note>();
        DateTime huy = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("d");
            Read();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (showing.SelectedItem != null)
            {
                Note selectNote = (Note)showing.SelectedItem;

                if (noteNam.Text != "" && desc.Text != "" && date.Text != "")
                {
                    selectNote.name = noteNam.Text;
                    selectNote.description = desc.Text;
                    selectNote.date = Convert.ToDateTime(date.Text);

                    Save_json();
                    Read();
                }
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (noteNam.Text == "" || desc.Text == "" || date.Text == "")
            { }
            else
            {
                Note newNote = new Note();
                newNote.name = noteNam.Text;
                newNote.description = desc.Text;
                newNote.date = Convert.ToDateTime(date.Text);
                noteList.Add(newNote);
                Save_json();
                Read();
            }
            showing.ItemsSource = noteList;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            noteList.Remove((Note)showing.SelectedItem);
            Save_json();
            Read();
            showing.ItemsSource = noteList;
        }
        void Read()
        {
            try
            {
                noteList = Json.Des<List<Note>>(date.Text + ".json");
                showing.ItemsSource = noteList;
            }
            catch
            {
                noteList = new List<Note>();
                showing.ItemsSource = noteList;
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Read();
        }

        void Save_json()
        {
            Json.Ser(noteList, date.Text + ".json");
        }

        private void showing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (showing.SelectedItem != null)
            {
                var selectNote = (Note)showing.SelectedItem;
                noteNam.Text = selectNote.name;
                desc.Text = selectNote.description;
                date.Text = selectNote.date.ToString();
            }
        }
    }
}