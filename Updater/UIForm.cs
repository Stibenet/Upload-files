using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using Updater.Utils;

namespace Updater
{
    /// <summary>
    ///     Summary description for Form1.
    /// </summary>
    public class UIForm : Form
    {
        protected const string SqlConnectionErrorMessage = "Отсутствует подключение к серверу. {}Строка подключения:{0}";
        private Label _lblItemCount;
        private ListBox _lstFiles;
        private MainMenu _mnu;
        private MenuItem _mnuAboout;
        private MenuItem _mnuExite;
        private OpenFileDialog _openFileDialog;
        private IContainer components;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem8;
        private MenuItem menuItem3;
        private Label label1;
        private Button button1;
        private const int BytesInMegabyte = 1048573;

        public UIForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private static string VersionStorageName
        {
            get { return ConfigurationSettings.AppSettings["VersionStorage"]; }
        }

        private static string ConnectionString
        {
            get { return ConfigurationSettings.AppSettings["ConnectionString"]; }
        }

        private static string StartUpFileName
        {
            get { return ConfigurationSettings.AppSettings["StartUpFile"]; }
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                TaskbarUtils.Init();

                var connectionChecker = new SqlConnectionChecker(ConnectionString);
                if (!connectionChecker.CheckResult)
                {
                    MessageBox.Show(connectionChecker.ErrorMessage, "Ошибка при проверке подключения к SQL серверу", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                else
                {
                    switch (args[0])
                    {
                        case "-u":
                            Application.Run(new UIForm());
                            break;
                    }
                }
            }
            catch (Exception exception)
            {

                string _errorMessage = string.Format("Ошибка. {0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace);
                MessageBox.Show("Ошибка запуска приложения:\r\n", _errorMessage);
            }

        }

        private static void ErrorMessage(Exception ex)
        {
            MessageBox.Show("Ошибка запуска приложения:\r\n" + ex, "Ошибка запуска приложения " + StartUpFileName);
        }

        private void UIForm_Load(object sender, EventArgs e)
        {
            InitOpenFileDialog();
            _openFileDialog.FileOk += _openFileDialog_FileOk;
        }

        #region Удаление файла из списка
        private void _lstFiles_DoubleClick(object sender, EventArgs e)
        {
            if (_lstFiles.SelectedIndex < 0)
            {
                return;
            }

            DialogResult result;
            result = MessageBox.Show(this, "Удалить файл из списка?", "Подтверждение удаления", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _lstFiles.Items.Remove(_lstFiles.Items[_lstFiles.SelectedIndex]);
            }

            _lstFiles.Refresh();
        }
        #endregion

        private void _mnuAboout_Click(object sender, EventArgs e)
        {
            var frm = new frmAbout();
            frm.ShowDialog(this);
            frm.Dispose();
        }


        #region Прерывание загрузки клавишей Escape
        private void UIForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
        #endregion

        #region Выбор файлов
        /// <summary>Настройка диалога</summary>
        private void InitOpenFileDialog()
        {
            _openFileDialog.Multiselect = true;
            _openFileDialog.Filter =
                "Исполняемые файлы (*.EXE;*.DLL)|*.EXE;*.DLL|" +
                "Файлы настроек (*.TXT;*.XML)|*.TXT;*.XML|" +
                "Все файлы (*.*)|*.*";
            _openFileDialog.FilterIndex = 3;
        }

        /// <summary>Возникает после нажатия на кнопку OK в диалоге</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            Activate();
            String[] files = _openFileDialog.FileNames;

            foreach (String file in files)
            {
                if (_lstFiles.FindStringExact(file) < 0)
                {
                    _lstFiles.Items.Add(file);
                }
            }

            _lblItemCount.Text = "Файлов выбрано: " + _lstFiles.Items.Count;
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIForm));
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._lstFiles = new System.Windows.Forms.ListBox();
            this._mnu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this._mnuExite = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this._mnuAboout = new System.Windows.Forms.MenuItem();
            this._lblItemCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lstFiles
            // 
            this._lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lstFiles.Location = new System.Drawing.Point(1, 30);
            this._lstFiles.Name = "_lstFiles";
            this._lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._lstFiles.Size = new System.Drawing.Size(534, 134);
            this._lstFiles.TabIndex = 2;
            this._lstFiles.SelectedIndexChanged += new System.EventHandler(this._lstFiles_SelectedIndexChanged);
            this._lstFiles.DoubleClick += new System.EventHandler(this._lstFiles_DoubleClick);
            // 
            // _mnu
            // 
            this._mnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem8,
            this._mnuExite});
            this.menuItem1.Text = "Файл";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Открыть";
            this.menuItem2.Click += new System.EventHandler(this._mnuOpen_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.Text = "Очистить";
            this.menuItem8.Click += new System.EventHandler(this._mnuClear_Click);
            // 
            // _mnuExite
            // 
            this._mnuExite.Index = 2;
            this._mnuExite.Text = "Выход";
            this._mnuExite.Click += new System.EventHandler(this._mnuExite_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._mnuAboout});
            this.menuItem3.Text = "Справка";
            // 
            // _mnuAboout
            // 
            this._mnuAboout.Index = 0;
            this._mnuAboout.Text = "О программе";
            this._mnuAboout.Click += new System.EventHandler(this._mnuAboout_Click);
            // 
            // _lblItemCount
            // 
            this._lblItemCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._lblItemCount.AutoSize = true;
            this._lblItemCount.Location = new System.Drawing.Point(4, 179);
            this._lblItemCount.Name = "_lblItemCount";
            this._lblItemCount.Size = new System.Drawing.Size(0, 13);
            this._lblItemCount.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Предварительный просмотр файлов перед загрузкой в БД";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(439, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Отправить в БД";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this._cmdUpLoad_Click);
            // 
            // UIForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(542, 204);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._lblItemCount);
            this.Controls.Add(this._lstFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Menu = this._mnu;
            this.Name = "UIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Работа с файлами базы данных";
            this.Load += new System.EventHandler(this.UIForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UIForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Buttons
        private void _mnuOpen_Click(object sender, EventArgs e)
        {
            _openFileDialog.ShowDialog(this);
        }

        private void _mnuDel_Click(object sender, EventArgs e)
        {
            foreach (object item in new ArrayList(_lstFiles.SelectedItems))
            {
                _lstFiles.Items.Remove(item);
            }
        }

        private void _mnuClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Очистить список файлов?", "Подтверждение удаления", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _lstFiles.Items.Clear();
            }
        }

        private void _mnuExite_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Загрузка выбранных файлов в БД

        /// <summary>Загрузка выбранных файлов в БД</summary>
        private void _cmdUpLoad_Click(object sender, EventArgs e)
        {
            if (_lstFiles.Items.Count == 0)
            {
                return;
            }

            String connString;
            connString = ConfigurationSettings.AppSettings["ConnectionString"];
            var FM = new FilesManager(connString);

            var progress = new frmProgress(_lstFiles.Items.Count);
            progress.Show();

            for (Int32 i = 0; i <= _lstFiles.Items.Count - 1; i++)
            {
                progress.Tick(1, _lstFiles.Items[i].ToString());
                FM.Upload(_lstFiles.Items[i].ToString());
            }

            progress.Close();
            _lstFiles.Items.Clear();


        }
        #endregion

        #region Boxs
        private void _lstInloadFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion
    }
}