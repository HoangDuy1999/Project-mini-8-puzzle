using _8_puzzle.Class;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace _8_puzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int time = 180;
        public DispatcherTimer Timer;
        ArrayList change_image = new ArrayList();
        public static bool run = false;
        public static int win = 0;
        public static int closes = 0;
        string filename = "";
        public static bool _yes_open = false;
        public static bool _yes_new_game = false;
        public static bool _yes_time_tick = false;
        List<matran> matrix = new List<matran>();
        public MainWindow()
        {
            InitializeComponent();
            // tạo mảng hai chều để hỗ trợ diều khiển 4 nút bấm
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix.Add(new matran() { i = i, j = j });
                }
            }
        }
        // HÀM HỖ TRỢ ĐỒNG HỒ ĐẾM NGƯỢC(CHẠY KHI NÚT START ẤN)

        void Timer_Tick(object sender, EventArgs e)
        {
            _yes_time_tick = true;
            if (win > 0)
            {
                time = 180;
                win = 0;
            }
            if (time > 0)
            {
                if (time <= 10)
                {
                    if (time % 2 == 0)
                        TBCountDown.Foreground = Brushes.Red;
                    else
                        TBCountDown.Foreground = Brushes.Black;
                    time--;
                    if (time % 60 > 9)
                        TBCountDown.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
                    else
                        TBCountDown.Text = string.Format("00:0{0}:0{1}", time / 60, time % 60);
                }
                else
                {
                    time--;
                    if (time % 60 > 9)
                        TBCountDown.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
                    else
                        TBCountDown.Text = string.Format("00:0{0}:0{1}", time / 60, time % 60);
                }
            }
            else
            {
                Timer.Stop();
                TBCountDown.Foreground = Brushes.Black;
                var screen = new game_over();
                screen.ShowDialog();
                run = false;
                win = 0;
                closes = 1;
            }
        }
        // START
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            run = true;
            Start.Width = 0;
            Start.Height = 0;
            TBCountDown.Width = 150;
            TBCountDown.Height = 50;
            End_game.Width = 150;
            End_game.Height = 35;
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
            ARROW_RIGHT.Width = 70;
            ARROW_LEFT.Width = 70;
            ARROW_UP.Width = 70;
            ARROW_DOWN.Width = 70;
            circle_center.Width = 70;
        }
        //END GAME
        private void End_game_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn muốn dùng cuộc chơi và thoát game?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Timer.Stop();
                Environment.Exit(0);
            }
        }
       
        // HELP
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1. Bạn có thể chơi game bằng cách ấn vô nut New game trước. Sau đó chọn image nhé." +
                            "\n2. Nếu bạn đã có file lưu game game thì chọn open để mở game và chơi tiếp tục nhé." +
                            "\n3. Để có thể di chuyển hình ảnh(hoặc dùng nút điều khiển), bạn hãy bấm vào nút start." +
                            "\n4. Ảnh có height quá lớn sẽ dẫn đến bị vỡ giao diện vậy nên bạn vui lòng chọn ảnh khác để chơi nhé." +
                            "\n5. Hỗ trợ 3 cách di chuyển ảnh: Dùng chuột di chuyển, ấn vào hình tự di chuyển đến ô trống, Dùng 4 nút di chuyển. ",
                            "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // SAVE
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (win == 1)
            {
                MessageBoxResult result = MessageBox.Show("Bạn đã win rồi. Nên không cần lưu nhé.", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (time == 0)
            {
                MessageBoxResult result = MessageBox.Show("Bạn đã Game over. Nên lưu làm gì.", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (time == 180)
            {
                MessageBoxResult result = MessageBox.Show("Bạn chưa bắt đầu chơi, nên chưa thể lưu game.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if ((time > 0 && win == 0) && time != 180)
            {
                ArrayList line = new ArrayList();
                Write_date_arrlist(line);
                // Set a variable to the Documents path.
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (line.Count > 0)
                {
                    // Write the string array to a new file named "WriteLines.txt".
                    using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, "My Favourite.txt")))
                    {
                        for (int i = 0; i < line.Count; i++)
                            outputFile.WriteLine(line[i].ToString());
                    }
                    MessageBox.Show("Đã lưu game thanh công vô file Favourite.txt => PATH: " + docPath.ToString() + "\\My Favourite.txt", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        // Ham hoox trojw save
        private void Write_date_arrlist(ArrayList line)
        {
            line.Add(filename + "_");
            for (int i = 0; i < change_image.Count - 1; i++)
            {
                //MessageBox.Show(change_image[i].ToString());
                line.Add(change_image[i]);
                line.Add("+");
            }
            line.Add(change_image[change_image.Count - 1]);
            line.Add("_");
            line.Add(time);
        }
        //Open
        OpenFileDialog openFileDialog = new OpenFileDialog();
        int tam = 0;
        private void Open_Click(object sender, RoutedEventArgs e)
        {
                openFileDialog.DefaultExt = ".txt";
                openFileDialog.Filter = "Text Document (.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    var url_prset = openFileDialog.FileName;

                    string s = File.ReadAllText(openFileDialog.FileName);

                    string[] array_open = s.ToString().Split('_');
                    // INSERT HÌNH VÔ
                    if (array_open.Count() == 3)
                    { 
                        images.Clear();
                        board.Children.Clear();
                        change_image.Clear();
                        win = 0;
                        run = false;
                        if (_yes_time_tick == true)
                        {
                            Timer.Stop();
                        }
                        var image = new BitmapImage(new Uri(array_open[0]));
                        _yes_open = true;
                        End_game.Width = 150;
                        End_game.Height = 35;
                         Original_image.Source = image;
                        if (image.PixelHeight > 710)
                        {
                            Thickness margin1 = End_game.Margin;
                            margin1.Top = 500;
                            End_game.Margin = margin1;
                            Thickness margin3 = TBCountDown.Margin;
                            margin3.Top = 450;
                            TBCountDown.Margin = margin3;
                        }
                       // Hiện các nút bấm cần thiết
                        time = int.Parse(array_open[2]);
                        Start.Width = 150;
                        Start.Height = 35;
                        End_game.Width = 0;
                        End_game.Height = 0;  
                        if(tam == 0)
                        {
                            TBCountDown.Width = 150;
                            TBCountDown.Height = 50;
                        }
                        else
                        {
                            TBCountDown.Width = 0;
                            TBCountDown.Foreground = Brushes.Black;
                        }
                        // Hiện 4 nut di chuyển
                        ARROW_DOWN.Width = 70;
                        ARROW_LEFT.Width = 70;
                        ARROW_RIGHT.Width = 70;
                        ARROW_UP.Width = 70;
                        circle_center.Width = 70;
                        circle_center.Width = 70;
                        // chia hình ảnh
                        var width = image.PixelWidth / 3;
                        var height = image.PixelHeight / 3;
                        var temp = 140 * height / width;
                        //reset lai cac mảng
                        images.Clear();
                        board.Children.Clear();
                        change_image.Clear();
                        // cat hinh
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                var cropped = new CroppedBitmap(image, new Int32Rect(i * width, j * height, width, height));
                                var img = new Image();
                                img.Source = cropped;
                                img.Width = 140;
                                img.Tag = i * 3 + j;
                                img.Height = temp;
                                images.Add(img);
                            }
                        }


                        string[] mangphu = array_open[1].ToString().Split('+');
                        for (int i = 0; i < mangphu.Count(); i++)
                        {
                            change_image.Add(int.Parse(mangphu[i]));
                        }

                        for (int i = 0; i < change_image.Count; i++)// đây là thứ tự các ảnh khi lưu game
                        {
                            // tìm ảnh giống với ảnh thứ i
                            for (int j = 0; j < images.Count; j++)
                            {
                                var change = change_image[i];
                                var imgs = images[j].Tag;
                                if ((int)change == (int)imgs && (int)change != -1) // đã tìm đúng ảnh
                                {
                                    // thêm vào canvas
                                    board.Children.Add(images[j]);
                                    // xác định tọa độ left top cho ảnh
                                    var left_update = 0;
                                    var top_update = 0;
                                    if (i == 0)
                                    {
                                        left_update = 0;
                                        top_update = 0;
                                    }
                                    if (i == 1)
                                    {
                                        left_update = 140 + 5;
                                        top_update = 0;
                                    }
                                    if (i == 2)
                                    {
                                        left_update = 2 * 140 + 10;
                                        top_update = 0;
                                    }
                                    if (i == 3)
                                    {
                                        left_update = 0;
                                        top_update = temp + 5;
                                    }
                                    if (i == 4)
                                    {
                                        left_update = 140 + 5;
                                        top_update = temp + 5;
                                    }
                                    if (i == 5)
                                    {
                                        left_update = 2 * 140 + 10;
                                        top_update = temp + 5;
                                    }
                                    if (i == 6)
                                    {
                                        left_update = 0;
                                        top_update = 2 * temp + 10;
                                    }
                                    if (i == 6)
                                    {
                                        left_update = 0;
                                        top_update = 2 * temp + 10;
                                    }
                                    if (i == 7)
                                    {
                                        left_update = 140 + 5;
                                        top_update = 2 * temp + 10;
                                    }
                                    if (i == 8)
                                    {
                                        left_update = 2 * 140 + 10;
                                        top_update = 2 * temp + 10;
                                    }
                                    // cập nhật top left cho ảnh vừa thêm vô canvas
                                    Canvas.SetLeft(images[j], left_update);
                                    Canvas.SetTop(images[j], top_update);
                                    // dếm sô lần open
                                   tam++;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("File không hợp lệ. Nạp game thất bại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
        }

        // NEW GAME
        List<Image> images = new List<Image>();
        private void New_game_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                filename = screen.FileName;
                try
                {
                    var image = new BitmapImage(new Uri(filename));
                    //reset
                    if(_yes_time_tick == true)
                    {
                        Timer.Stop();
                        TBCountDown.Width = 0;
                        TBCountDown.Foreground = Brushes.Black;
                        End_game.Width = 0;
                        run = false;
                        win = 0;
                    }
                    board.Children.Clear();
                    change_image.Clear();
                    images.Clear();
                    // hien nut start
                    if (image.PixelHeight > 710)
                    {
                        Thickness margin = Start.Margin;
                        margin.Top = 500;
                        Start.Margin = margin;
                        Thickness margin1 = End_game.Margin;
                        margin1.Top = 500;
                        End_game.Margin = margin1;
                        Thickness margin3 = TBCountDown.Margin;
                        margin3.Top = 450;
                        TBCountDown.Margin = margin3;
                    }
                    Start.Width = 150;
                    Start.Height = 35;
                    time = 180;
                    var width = image.PixelWidth / 3;
                    var height = image.PixelHeight / 3;
                    var temp = 140 * height / width;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            var cropped = new CroppedBitmap(image, new Int32Rect(i * width, j * height, width, height));
                            var img = new Image();
                            img.Source = cropped;
                            img.Width = 140;
                            img.Tag = i * 3 + j;
                            img.Height = temp;
                            images.Add(img);

                        }
                    }
                    var rd = new Random();
                    var indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
                    for (int i = 0; i < 3; i++)
                    {
                        for (var j = 0; j < 3; j++)
                        {
                            if (!(i == 2 && j == 2))
                            {
                                var index = rd.Next(indices.Count);
                                var index2 = indices[index];
                                board.Children.Add(images[index2]);
                                change_image.Add(images[index2].Tag);
                                Canvas.SetLeft(images[index2], j * (140 + 5));
                                Canvas.SetTop(images[index2], i * (temp + 5));
                                indices.RemoveAt(index);
                            }
                        }
                    }
                    change_image.Add(-1); // ma tran o duoi
                    Original_image.Source = image;
                }catch
                {
                    MessageBox.Show("file bạn chọn không phải hình.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // DI CHUYỂN ẢNH
        bool _isDragging = false;
        Point _lastPos;
        int _selectedImageindex = -1;
        // BỘ 3 HUYỀN THOẠI
        private void Board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (run == true)
            {
                _isDragging = true;
                _lastPos = e.GetPosition(this);
                for (int i = 0; i < images.Count; i++)
                {
                    var left = Canvas.GetLeft(images[i]);
                    var top = Canvas.GetTop(images[i]) + 55;
                    
                    if ((_lastPos.X > left && _lastPos.X < left + (int)images[i].Width) && (_lastPos.Y > top && _lastPos.Y < top + (int)images[i].Height))
                    {

                        _selectedImageindex = i;
                        // MessageBox.Show(images[_selectedImageindex].Tag.ToString());
                        Title = $"{i + " - "}{change_image[_selectedImageindex].ToString()}{"  -  "} { images[_selectedImageindex].Tag}";
                    }
                }
            }
        }
        private void Board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           if(_isDragging && run == true)
            {
                _isDragging = false;
                _lastPos = e.GetPosition(this);
                int width = (int)images[_selectedImageindex].Width;
                int height = (int)images[_selectedImageindex].Height;
                int i1 = 0;
                int j1 = 0;
                var pos = e.GetPosition(this);
                for (int k = 0; k < change_image.Count; k++)
                {
                    if ((int)change_image[k] == _selectedImageindex)
                        i1 = k;
                    if ((int)change_image[k] == -1)
                        j1 = k;
                }
               // MessageBox.Show("i: " + i1 + " j: " + j1);
                change_image[j1] = _selectedImageindex;
                change_image[i1] = -1;
                //for (int i = 0; i < change_image.Count; i++)
                //{
                //    MessageBox.Show(change_image[i].ToString());
                //}
                int left_update = 0;
                int top_update = 0;
                if(j1 == 0)
                {
                    left_update = 0;
                    top_update = 0;
                }
                if(j1 == 1)
                {
                    left_update = width + 5;
                    top_update = 0;
                }
                if(j1 == 2)
                {
                    left_update = 2* width + 10;
                    top_update = 0;
                }
                if (j1 == 3)
                {
                    left_update = 0;
                    top_update = height + 5;
                }
                if(j1 == 4)
                {
                    left_update = width + 5;
                    top_update = height + 5;
                }
                if (j1 == 5)
                {
                    left_update = 2 * width + 10;
                    top_update =height + 5;
                }
                if (j1 == 6)
                {
                    left_update = 0;
                    top_update = 2* height + 10;
                }
                if (j1 == 7)
                {
                    left_update = width + 5;
                    top_update = 2 * height + 10;
                }
                if (j1 == 8)
                {
                    left_update = 2 * width + 10;
                    top_update = 2 * height + 10;
                }
                Canvas.SetLeft(images[_selectedImageindex], left_update);
                Canvas.SetTop(images[_selectedImageindex], top_update);
                check_win();
            }
        }
        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && run == true)
            {
                var curPos = e.GetPosition(this);

                var dx = curPos.X - _lastPos.X;
                var dy = curPos.Y - _lastPos.Y;
                var img = images[_selectedImageindex];
                var oldLeft = Canvas.GetLeft(img);
                var oldTop = Canvas.GetTop(img);
                Canvas.SetLeft(img, oldLeft + dx);
                Canvas.SetTop(img, oldTop + dy);
                _lastPos = curPos;
            }
        }

        // kiểm tra win
        private void check_win()
        {
            if((int)change_image[0] == 0 && (int)change_image[1] == 3 && (int)change_image[2] == 6 
                && (int)change_image[3] == 1 && (int)change_image[4] == 4 && (int)change_image[5] == 7
                && (int)change_image[6] == 2 && (int)change_image[7] == 5)
            {
                Timer.Stop();
                run = false;
                win = 1;
                var sreen = new you_win(filename);
                sreen.ShowDialog();
                Start.Width = 0;
                End_game.Width = 150;
                End_game.Height = 35;
            }
        }

        // auto with
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size_change_tool_bar.Width = size_window.Width;
            board.Height = size_window.Height;
            background_right.Height = size_window.Height;
        }

        // Tìm vi trí của ô null
        int left_down = 0;
        int top_down = 0;
        private void Vi_Tri_Moi(int j1)
        {
            int width = (int)images[0].Width;
            int height = (int)images[0].Height;
            if (j1 == 0)
            {
                left_down = 0;
                top_down = 0;
            }
            if (j1 == 1)
            {
                left_down = width + 5;
                top_down = 0;
            }
            if (j1 == 2)
            {
                left_down = 2 * width + 10;
                top_down = 0;
            }
            if (j1 == 3)
            {
                left_down = 0;
                top_down = height + 5;
            }
            if (j1 == 4)
            {
                left_down = width + 5;
                top_down = height + 5;
            }
            if (j1 == 5)
            {
                left_down = 2 * width + 10;
                top_down = height + 5;
            }
            if (j1 == 6)
            {
                left_down = 0;
                top_down = 2 * height + 10;
            }
            if (j1 == 7)
            {
                left_down = width + 5;
                top_down = 2 * height + 10;
            }
            if (j1 == 8)
            {
                left_down = 2 * width + 10;
                top_down = 2 * height + 10;
            }
        }

        // BỐN HÀM DI CHUYỂN Ở DƯỚI
        private void ARROW_UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (run == true && win == 0)
            {
                int vt_null = 0;
                int vt_doicho = -1;
                for (int i = 0; i < change_image.Count; i++)
                {
                    if ((int)change_image[i] == -1)
                    {
                        vt_null = i;
                        // tìm ai nằm trên ô trống (i = i + 1, j)
                        if (matrix[i].i + 1 <= 2) // nếu trừ ra < 0 thì nó nằm ở dòng - 1(sai)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if ((matrix[j].i == matrix[i].i + 1) && matrix[j].j == matrix[i].j)// dựa vào tọa dộ ta tìm dược ví trí ảnh năm trên
                                {
                                    vt_doicho = j;
                                }
                            }
                        }
                    }
                }

                if (vt_doicho != -1)
                {
                    int tag = (int)change_image[vt_doicho]; // cái hình cai thay đổi  vô ô trống
                                                            // cái gia trị left top của cái ô trống
                    Vi_Tri_Moi(vt_null);
                    change_image[vt_null] = change_image[vt_doicho];
                    change_image[vt_doicho] = -1;
                    // tìm cái hình cần di chuyển vô ô trống
                    // tìm vị trí số thư tuwh images
                    int vt_hinh = 0;
                    for (int i = 0; i < images.Count; i++)
                    {
                        if ((int)images[i].Tag == tag)
                        {
                            vt_hinh = i;
                        }
                    }
                    Canvas.SetLeft(images[vt_hinh], left_down);
                    Canvas.SetTop(images[vt_hinh], top_down);
                    check_win();
                }
            }
        }

        private void ARROW_DOWN_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (run == true && win == 0)
            {
                int vt_null = 0;
                int vt_doicho = -1;
                for (int i = 0; i < change_image.Count; i++)
                {
                    if ((int)change_image[i] == -1)
                    {
                        vt_null = i;
                        // tìm ai nằm trên ô trống (i = i - 1, j)
                        if (matrix[i].i - 1 >= 0) // nếu trừ ra < 0 thì nó nằm ở dòng - 1(sai)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if ((matrix[j].i == matrix[i].i - 1) && matrix[j].j == matrix[i].j)// dựa vào tọa dộ ta tìm dược ví trí ảnh năm trên
                                {
                                    vt_doicho = j;
                                }
                            }
                        }
                    }
                }

                if (vt_doicho != -1)
                {
                    int tag = (int)change_image[vt_doicho]; // cái hình cai thay đổi  vô ô trống
                                                            // cái gia trị left top của cái ô trống
                    Vi_Tri_Moi(vt_null);
                    change_image[vt_null] = change_image[vt_doicho];
                    change_image[vt_doicho] = -1;
                    // tìm cái hình cần di chuyển vô ô trống
                    // tìm vị trí số thư tuwh images
                    int vt_hinh = 0;
                    for (int i = 0; i < images.Count; i++)
                    {
                        if ((int)images[i].Tag == tag)
                        {
                            vt_hinh = i;
                        }
                    }
                    Canvas.SetLeft(images[vt_hinh], left_down);
                    Canvas.SetTop(images[vt_hinh], top_down);
                    check_win();
                }
            }
        }

        private void ARROW_LEFT_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (run == true && win == 0)
            {
                int vt_null = 0;
                int vt_doicho = -1;
                for (int i = 0; i < change_image.Count; i++)
                {
                    if ((int)change_image[i] == -1)
                    {
                        vt_null = i;
                        // tìm ai nằm trên ô trống (i, j - 1)
                        if (matrix[i].j + 1 <= 2) // nếu trừ ra < 0 thì nó nằm ở dòng - 1(sai)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if ((matrix[j].j == matrix[i].j + 1) && matrix[j].i == matrix[i].i)// dựa vào tọa dộ ta tìm dược ví trí ảnh năm trên
                                {
                                    vt_doicho = j;
                                }
                            }
                        }
                    }
                }

                if (vt_doicho != -1)
                {
                    int tag = (int)change_image[vt_doicho]; // cái hình cai thay đổi  vô ô trống
                                                            // cái gia trị left top của cái ô trống
                    Vi_Tri_Moi(vt_null);
                    change_image[vt_null] = change_image[vt_doicho];
                    change_image[vt_doicho] = -1;
                    // tìm cái hình cần di chuyển vô ô trống
                    // tìm vị trí số thư tuwh images
                    int vt_hinh = 0;
                    for (int i = 0; i < images.Count; i++)
                    {
                        if ((int)images[i].Tag == tag)
                        {
                            vt_hinh = i;
                        }
                    }
                    Canvas.SetLeft(images[vt_hinh], left_down);
                    Canvas.SetTop(images[vt_hinh], top_down);
                    check_win();
                }
            }
        }

        private void ARROW_RIGHT_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (run == true && win == 0)
            {
                int vt_null = 0;
                int vt_doicho = -1;
                for (int i = 0; i < change_image.Count; i++)
                {
                    if ((int)change_image[i] == -1)
                    {
                        vt_null = i;
                        // tìm ai nằm trên ô trống (i, j - 1)
                        if (matrix[i].j - 1 >= 0) // nếu trừ ra < 0 thì nó nằm ở dòng - 1(sai)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if ((matrix[j].j == matrix[i].j - 1) && matrix[j].i == matrix[i].i)// dựa vào tọa dộ ta tìm dược ví trí ảnh năm trên
                                {
                                    vt_doicho = j;
                                }
                            }
                        }
                    }
                }

                if (vt_doicho != -1)
                {
                    int tag = (int)change_image[vt_doicho]; // cái hình cai thay đổi  vô ô trống
                                                            // cái gia trị left top của cái ô trống
                    Vi_Tri_Moi(vt_null);
                    change_image[vt_null] = change_image[vt_doicho];
                    change_image[vt_doicho] = -1;
                    // tìm cái hình cần di chuyển vô ô trống
                    // tìm vị trí số thư tuwh images
                    int vt_hinh = 0;
                    for (int i = 0; i < images.Count; i++)
                    {
                        if ((int)images[i].Tag == tag)
                        {
                            vt_hinh = i;
                        }
                    }
                    Canvas.SetLeft(images[vt_hinh], left_down);
                    Canvas.SetTop(images[vt_hinh], top_down);
                    check_win();
                }
            }
        }
    }
}
