using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//чтобы работали path и file
using Npgsql;//база данных

namespace NewDemoex
{
    public partial class LoginForm : Form
    {
        // соединение с бд через NPGSQL
        public string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=dbd";
        private int panelTopOffset = 10;
        private Panel applicationsContainerPanel;
        public LoginForm()
        {
            InitializeComponent();
            //Базовые параметры окна и иконка
            this.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            this.Text = "Вход в систему Обувь для Солевых";
            //this.Icon = new Icon("Icon.ico");
            this.Size = new Size(500, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            //Логотипные настройки
            PictureBox logoPictureBox = new PictureBox();
            logoPictureBox.Image = Image.FromFile("Icon.png");
            logoPictureBox.Size = new Size(100, 100);
            logoPictureBox.Location = new Point(180, 10);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(logoPictureBox);

            // Метка для логина
            Label lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(145, 115);
            lblLogin.AutoSize = true;
            this.Controls.Add(lblLogin);  // добавляем в LoginForm

            // Поле для логина
            TextBox txtLogin = new TextBox();
            txtLogin.Location = new Point(145, 130);
            txtLogin.Width = 200;
            this.Controls.Add(txtLogin);  // добавляем в LoginForm

            // Метка для пароля
            Label lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(145, 155);
            lblPassword.AutoSize = true;
            this.Controls.Add(lblPassword);  // добавляем в LoginForm

            // Поле для пароля
            TextBox txtPassword = new TextBox();
            txtPassword.Location = new Point(145, 170);
            txtPassword.Width = 200;
            txtPassword.UseSystemPasswordChar = true;
            this.Controls.Add(txtPassword);  // добавляем в LoginForm

            // Кнопка входа
            Button btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new Point(170, 210);
            btnLogin.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
            btnLogin.Size = new Size(150, 50);
            this.Controls.Add(btnLogin);  // добавляем в LoginForm

            // Кнопка входа Гостя
            Button btnLoginG = new Button();
            btnLoginG.Text = "Войти как Гость";
            btnLoginG.Location = new Point(170, 432);
            btnLoginG.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
            btnLoginG.Size = new Size(150, 50);
            this.Controls.Add(btnLoginG);  // добавляем в LoginForm

            //Основная форма
            Form MainForm = new Form();
            MainForm.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            MainForm.Text = "Список товаров";
            //MainForm.Icon = new Icon("Icon.ico");
            MainForm.Size = new Size(2048, 1080);
            MainForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            MainForm.StartPosition = FormStartPosition.CenterScreen;

            PictureBox logoPB = new PictureBox();
            logoPB.Image = Image.FromFile("Icon.png");
            logoPB.Size = new Size(150, 150);
            logoPB.Location = new Point(10, 10);
            logoPB.SizeMode = PictureBoxSizeMode.StretchImage;
            MainForm.Controls.Add(logoPB);

            applicationsContainerPanel = new Panel();
            applicationsContainerPanel.Location = new Point(10, 170);
            applicationsContainerPanel.Size = new Size(860, 700);
            applicationsContainerPanel.BorderStyle = BorderStyle.FixedSingle;
            applicationsContainerPanel.AutoScroll = true; //Скроллинг

            MainForm.Controls.Add(applicationsContainerPanel);


            LoadApplications();

            void LoadApplications()
            {
                // конструкция основной функции с панелями и другим в формате отлова потенциальных ошибок
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        // открывает коннект, запрашиваем данные которые потом используем
                        connection.Open();
                        string query = @"
                        SELECT
                        p.id,
                        p.name,
                        p.photo,
                        p.description,
                        m.name AS manufacture_name,  -- название производителя
                        s.name AS supplier_name,    -- название поставщика
                        p.price,
                        p.unit,
                        p.stock,
                        c.name AS category_name,    -- название категории
                        p.discount,
                        p.squ
                        FROM products p
                        JOIN manufactures m ON p.manufacterid = m.id
                        JOIN suppliers s ON p.suppliersid = s.id
                        JOIN categories c ON p.categoryid = c.id;";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            // читаем получаенные данные и вставляем
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                // переменные для удобной настройки положения штук в окне
                                int panelHeight = 100;
                                int panelSpacing = 10;
                                panelTopOffset = 10;

                                applicationsContainerPanel.Controls.Clear();

                                // текст с названием компании
                                Label centerLabel = new Label();
                                centerLabel.Text = "Обувь для Солевых";
                                centerLabel.Font = new Font("Bahnschrift Light SemiCondensed", 24);
                                centerLabel.AutoSize = true;
                                MainForm.Controls.Add(centerLabel);
                                centerLabel.Location = new Point(160, 30);
                                // текст над армией буклетов компаний
                                Label qLabel = new Label();
                                qLabel.Text = $"Ассортимент для вас!";
                                qLabel.Font = new Font("Bahnschrift Light SemiCondensed", 16);
                                qLabel.AutoSize = true;
                                MainForm.Controls.Add(qLabel);
                                qLabel.Location = new Point(360, 139);

                                // Цикл с графикой и занесением данных в панели
                                while (reader.Read())
                                {
                                    // создаем панельку
                                    Panel applicationPanel = new Panel();
                                    applicationPanel.Font = new Font("Bahnschrift Light SemiCondensed", 24);
                                    applicationPanel.Size = new Size(820, panelHeight);
                                    applicationPanel.Location = new Point(10, panelTopOffset);
                                    applicationPanel.BorderStyle = BorderStyle.FixedSingle;

                                    panelTopOffset += panelHeight + panelSpacing;
                                    // Создаем переменные и лейбл с названием товара и производителя
                                    
                                    //Картинка...
                                    PictureBox photoPictureBox = new PictureBox();
                                    string photoFileName = reader.GetString(2); // например, "pic.png"
                                    string imagePath = Path.Combine(Application.StartupPath, photoFileName);
                                    if (File.Exists(imagePath))
                                    {
                                        photoPictureBox.Image = Image.FromFile(imagePath);
                                    }
                                    else
                                    {
                                        //Если не прогрузились картинки то заменим на лого
                                        PictureBox logoPictureBox = new PictureBox();
                                        logoPictureBox.Image = Image.FromFile("pic.png");
                                        logoPictureBox.Size = new Size(50, 50);
                                        logoPictureBox.Location = new Point(10, 10);
                                        logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                        applicationPanel.Controls.Add(logoPictureBox);
                                    }
                                    photoPictureBox.Size = new Size(50, 50);
                                    photoPictureBox.Location = new Point(10, 10);
                                    photoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                    applicationPanel.Controls.Add(photoPictureBox);
                                    //Основная инфа
                                    Label titleLabel = new Label();
                                    string productName = reader.GetString(1);
                                    string manufactureName = reader.GetString(4);
                                    string suppliersName = reader.GetString(5);
                                    titleLabel.Text = productName + $" ({suppliersName} - {manufactureName})";
                                    titleLabel.Size = new Size(780, 30);
                                    titleLabel.Location = new Point(76, 10);
                                    titleLabel.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                                    applicationPanel.Controls.Add(titleLabel);
                                    // Создаем переменные и лейбл с деталями: описание, категория, производство, цена и т.п.
                                    string description = reader.GetString(3);
                                    string categoryName = reader.GetString(9);
                                    decimal price = reader.GetDecimal(6);
                                    string unit = reader.GetString(7);
                                    int stock = reader.GetInt32(8);
                                    decimal disc = reader.GetDecimal(10);
                                    //выделение цветом с условием
                                    if (stock == 0)
                                    {
                                        applicationPanel.BackColor = ColorTranslator.FromHtml("#87CEEB");
                                    }
                                    else if (disc > 15)
                                    {
                                        applicationPanel.BackColor = ColorTranslator.FromHtml("#B22222");
                                    }
                                    //отображение цены со скидкой или без
                                    if (disc > 0)
                                    {
                                        decimal finalPrice = price * (disc / 100);
                                        price = price - finalPrice;

                                    }
                                    else
                                    {
                                        price = reader.GetDecimal(6);
                                    }
                                    Label detailsLabel = new Label();
                                    detailsLabel.Text = $"{description}, Категория: {categoryName}, Остаток: {stock} {unit}, Цена со скидкой: {price:F2} ₽, | Скидка: {disc}%";
                                    detailsLabel.Size = new Size(780, 40);
                                    detailsLabel.Location = new Point(76, 50);
                                    detailsLabel.Font = new Font("Bahnschrift Light SemiCondensed", 9);
                                    applicationPanel.Controls.Add(detailsLabel);
                                    //призываем демона с буклетами
                                    applicationsContainerPanel.Controls.Add(applicationPanel);
                                   

                                }
                            }
                        }
                        //Выход в логинформу
                        Button returnToLogin = new Button();
                        returnToLogin.Text = "Выйти";
                        returnToLogin.Location = new Point(1670, 850);
                        returnToLogin.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                        returnToLogin.Size = new Size(150, 100);
                        MainForm.Controls.Add(returnToLogin);
                        returnToLogin.Click += returnToLogin_Click;
                        void returnToLogin_Click(object sender, EventArgs e)
                        {
                            MessageBox.Show("Выход из системы. Переход в окно авторизации");
                            MainForm.Hide();
                            this.Show();
                        }

                        //Типо сортировка-заглушка
                        ComboBox OptList = new ComboBox();
                        OptList.DropDownStyle = ComboBoxStyle.DropDownList;
                        OptList.Size = new Size(300, 64);
                        OptList.Location = new Point(1250, 160);
                        OptList.Items.Insert(0, "Сортировка");
                        OptList.SelectedIndex = 0;
                        OptList.Font = new Font("Bahnschrift Light SemiCondensed", 12);
                        OptList.Items.Add("Kari");
                        OptList.Items.Add("Обувь для вас");
                        MainForm.Controls.Add(OptList);

                        //Типо поиск-заглушка
                        Label srchLbl = new Label();
                        srchLbl.Text = "Вы можете найти нужную заявку вручную:";
                        srchLbl.AutoSize = true;
                        srchLbl.Font = new Font("Bahnschrift Light SemiCondensed", 12);
                        srchLbl.Location = new Point(1250, 260);
                        MainForm.Controls.Add(srchLbl);
                        TextBox SearchTB = new TextBox();
                        SearchTB.Size = new Size(300, 64);
                        SearchTB.AutoSize = true;
                        SearchTB.Font = new Font("Bahnschrift Light SemiCondensed", 12);
                        SearchTB.Location = new Point(1250, 280);
                        MainForm.Controls.Add(SearchTB);

                        Button buttonSearch = new Button();
                        buttonSearch.Size = new Size(100, 34);
                        buttonSearch.Location = new Point(1340, 320);
                        buttonSearch.Text = "Найти";
                        buttonSearch.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                        buttonSearch.Click += (cs, ce) => MessageBox.Show("Кажется что-то пошло не так...");
                        MainForm.Controls.Add(buttonSearch);

                        //Поиск и сортировка функции
                        

                        // кнопка ведущая к другому окну с другой функцией
                        Button button1 = new Button();
                        button1.Size = new Size(300, 64);
                        button1.Location = new Point(250, 860);
                        button1.Text = "Добавить/Удалить данные";
                        button1.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                        MainForm.Controls.Add(button1);
                        // Создает новую функцию и форму, закрывает первое окно
                        button1.Click += Button1_Click;
                        void Button1_Click(object sender, EventArgs e)
                        {
                            // настройки окна
                            MainForm.Hide();
                            Form EdDate = new Form();
                            EdDate.Text = "Работа с данными";
                            EdDate.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                            EdDate.Size = new Size(800, 400);
                            EdDate.StartPosition = FormStartPosition.CenterScreen;
                            EdDate.FormBorderStyle = FormBorderStyle.FixedDialog;
                            // текст
                            Label messageLabel = new Label();
                            messageLabel.Text = "Что вы хотите сделать?";
                            messageLabel.AutoSize = true;
                            messageLabel.Font = new Font("Bahnschrift Light SemiCondensed", 14);
                            messageLabel.Location = new Point(290, 20);
                            EdDate.Controls.Add(messageLabel);
                            // редактировать данные кнопка
                            Button EdBtn = new Button();
                            EdBtn.Text = "Редактировать данные";
                            EdBtn.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                            EdBtn.Size = new Size(250, 50);
                            EdBtn.Location = new Point(265, 80);
                            EdBtn.Click += EdBtn_Click;
                            void EdBtn_Click(object sender, EventArgs e)
                            {
                                MessageBox.Show("Временно не работает. Возврат с карточкам");
                                EdDate.Hide();
                                MainForm.Show();
                            }
                            // кнопка удаления данных
                            Button delBtn = new Button();
                            delBtn.Text = "Удалить данные";
                            delBtn.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                            delBtn.Size = new Size(250, 50);
                            delBtn.Location = new Point(265, 200);
                            delBtn.Click += delBtn_Click;
                            void delBtn_Click(object sender, EventArgs e)
                            {
                                MessageBox.Show("Временно не работает. Возврат с карточкам");
                                EdDate.Hide();
                                MainForm.Show();
                            }

                            // кнопка возврата к карточками
                            Button closeBtn = new Button();
                            closeBtn.Text = "Вернуться к карточкам";
                            closeBtn.Font = new Font("Bahnschrift Light SemiCondensed", 12, FontStyle.Bold);
                            closeBtn.Size = new Size(250, 50);
                            closeBtn.Location = new Point(265, 300);
                            closeBtn.Click += closeBtn_Click;
                            void closeBtn_Click(object sender, EventArgs e)
                            {
                                MessageBox.Show("Возврат с карточкам");
                                EdDate.Hide();
                                MainForm.Show();
                            }
                            // выше уже все пояснено
                            EdDate.Controls.Add(EdBtn);
                            EdDate.Controls.Add(delBtn);
                            EdDate.Controls.Add(closeBtn);
                            EdDate.ShowDialog();
                            
                        }
                    }
                }
                catch (Exception ex) // вывод ошибок в боксе  если будут
                {
                    MessageBox.Show(ex.Message);
                }
            }

            btnLoginG.Click += btnLoginG_Click;
            void btnLoginG_Click(object sender, EventArgs e)
            {
                MessageBox.Show("Залогинься, будь человеком.");
                this.Hide();
                MainForm.Show();
            }

            btnLogin.Click += btnLogin_Click;
            void btnLogin_Click(object sender, EventArgs e)
            {
                string login = txtLogin.Text;
                string password = txtPassword.Text;

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"
                        SELECT users.fullname, roles.name
                        FROM users
                        JOIN roles ON users.roleid = roles.id
                        WHERE users.login = @login AND users.password = @password
                        LIMIT 1";

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("login", login);
                            cmd.Parameters.AddWithValue("password", password);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string fullname = reader["fullname"].ToString();
                                    string roleName = reader["name"].ToString();

                                    MessageBox.Show($"Успешная авторизация!\nПользователь: {fullname}\nРоль: {roleName}");

                                    this.Hide();
                                    MainForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Неверный логин или пароль! Сосать!");
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при подключении к базе данных. Реально сосал");
                    }
                }
            }

            // тут была не работающая кнопкахвхвахавх гостяавххавхвахв...

        }
    }
}