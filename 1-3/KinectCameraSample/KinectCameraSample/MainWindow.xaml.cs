using System;
using System.Collections.Generic;
using System.Linq;
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

using Microsoft.Kinect;


namespace KinectCameraSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private byte[] pixelBuffer = null;
        private WriteableBitmap bmpBuffer = null;


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Kinectセンサーの取得
            KinectSensor kinect = KinectSensor.KinectSensors[0];

            // カラーストリームの有効化
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            // バッファの初期化
            pixelBuffer = new byte[kinect.ColorStream.FramePixelDataLength];
            bmpBuffer = new WriteableBitmap(kinect.ColorStream.FrameWidth,
                kinect.ColorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null);

            rgbImage.Source = bmpBuffer;

            // イベントハンドラの登録
            kinect.ColorFrameReady += ColorImageReady;

            // Kinectセンサーからのストリーム取得を開始
            kinect.Start();
        }




        /// <summary>
        /// カラーストリームのデータ更新イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColorImageReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            try
            {
                // 更新データを取得する
                using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
                {
                    if (imageFrame != null)
                    {
                        // 画像情報をバッファにコピー
                        imageFrame.CopyPixelDataTo(pixelBuffer);

                        // ビットマップに描画
                        Int32Rect src = new Int32Rect(0, 0, imageFrame.Width, imageFrame.Height);
                        bmpBuffer.WritePixels(src, pixelBuffer, imageFrame.Width * 4, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
