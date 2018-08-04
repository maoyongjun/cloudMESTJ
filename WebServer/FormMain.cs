using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp.Server;
using WebServer.SocketService;

namespace WebServer
{
    public partial class FormMain : Form
    {        
        WebSocketServer SFCSocket;
        delegate void UpdateControls(string msg, int count);
        Dictionary<string, List<SessionIten>> session = new Dictionary<string,List< SessionIten>>();
        public FormMain()
        {
            InitializeComponent();
            SFCSocket = new WebSocketServer(System.Net.IPAddress.Any, 2130);
            SFCSocket.Log.Level = WebSocketSharp.LogLevel.Debug;
            SFCSocket.WaitTime = TimeSpan.FromSeconds(2);
            SFCSocket.AddWebSocketService<Report>("/ReportService", new Func<Report>(NewReportService));
            SFCSocket.Start();

        }

        private Report NewReportService()
        {
            Report _ReportService = new Report();
            _ReportService.OnSocketOpen += _ReportService_OnSocketOpen;
            _ReportService.OnSocketClose += _ReportService_OnSocketClose;
            _ReportService.OnSocketError += _ReportService_OnSocketError;
            _ReportService.OnSocketMessage += _ReportService_OnSocketMessage;
            return _ReportService;
        }

        private void _ReportService_OnSocketMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            Report s = (Report)sender;
            
            UpdateControl(s.ClientIP+":"+s.ClientPort + ":" + s.ID + ">>>" + e.Data + " \r\n", 1);
            //1 将输入处理成 KEY：value的关系

            //

            //2 判断调用类型

            //3 获取处理请求对象
                //3.1如果当前用户没有创建过就New一个。
                
                //3.2如果已经有现成的，就获取

            //4调用处理方法

            //5返回处理结果
        }

        private void _ReportService_OnSocketOpen(object sender, EventArgs e)
        {
            Report s = (Report)sender;
            UpdateControl(s.ClientIP + ":" + s.ClientPort + ":" + s.ID + ">>> Connetion Open \r\n", 1);
        }

        private void _ReportService_OnSocketClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            Report s = (Report)sender;
            UpdateControl(s.ClientIP + ":" + s.ClientPort + ":" + s.ID + ">>> Connetion Close \r\n", -1);
        }
        private void _ReportService_OnSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Report s = (Report)sender;
            UpdateControl(s.ClientIP + ":" + s.ClientPort + ":" + s.ID + ">>>" + e.Message + "\r\n", 0);
        }

        void UpdateControl(string msg, int count)
        {
            if (lab_Count.InvokeRequired)
            {
                Action<int> actionDelegate = (x) => {
                    lock (lab_Count)
                    {
                        lab_Count.Text = (Convert.ToInt32(lab_Count.Text) + x).ToString();
                    }
                };
                lab_Count.BeginInvoke(actionDelegate, count);
            }
            //else
            //{
            //    lock (lab_Count)
            //    {
            //        lab_Count.Text = (Convert.ToInt32(lab_Count.Text) + count).ToString();
            //    }
            //}
            if (txt_MSG.InvokeRequired)
            {
                Action<string> actionDelegate = (x) =>
                {
                    lock (lab_Count)
                    {
                        txt_MSG.Text = x + txt_MSG.Text;
                    }
                };
                txt_MSG.BeginInvoke(actionDelegate, msg);
            }
            //else
            //{
            //    lock (lab_Count)
            //    {
            //        txt_MSG.Text = msg + txt_MSG.Text;
            //    }
            //}
        }
    }
    public class SessionIten
    {
        public string key;
        public object Value;
    }
}
