using System;
using System.Windows;
using System.Drawing; //Color, bitmap, Graphics
using System.Windows.Forms; //Screen height and width
using MessageBox = System.Windows.MessageBox;// Use message box of wpf (not forms)
using System.Runtime.InteropServices; // User32.dll (and dll import)



namespace PixelBot
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx,uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        }

        private void OnButtonSearchPixelClick(object sender, RoutedEventArgs e)
        {
            string inputHexColorCode = TextBoxHexColor.Text;
            
            SearchPixel(inputHexColorCode);
        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            System.Threading.Thread.Sleep(250);
            Click();
        }

        private bool SearchPixel(string hexCode)
        {
            //Create an empty bitmap with the size of the current screen
            //Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //Create an empty bitmap with the size of all connected screen
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            //Create a new graphics objects that can capture the screen
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            //Screenshot moment -> screen content to graphics object
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            // e.g. tranlate #ffffff to a Color object
            Color desiredPixelColor = ColorTranslator.FromHtml(hexCode);

            for(int x = 0; x < SystemInformation.VirtualScreen.Width; x++)
            {
                for(int y = 0; y < SystemInformation.VirtualScreen.Height; y++)
                {
                    //Get the current pixels color
                    Color currentPixelColor = bitmap.GetPixel(x, y);

                    //Compare the pixels hex color and desired hex color (if they match we found a pixel)
                    if(desiredPixelColor == currentPixelColor)
                    {
                        MessageBox.Show(String.Format("Found Pixel at {0},{1} - Now set mouse cursor", x, y));

                        //TODO set mouse cursor and double click
                        DoubleClickAtPosition(x, y);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
