using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridViewColumns();
        }

        private void InitializeDataGridViewColumns()
        {
            if (dataGridView1 != null)
            {
                // Очищаем старые столбцы перед добавлением новых
                dataGridView1.Columns.Clear();

                // Код лексемы
                this.dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = "TokenCode",
                    HeaderText = "Код лексемы",
                    Width = 100
                });

                // Тип лексемы (ключевое слово, идентификатор, оператор и т. д.)
                this.dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = "TokenType",
                    HeaderText = "Тип лексемы",
                    Width = 150
                });

                // Сама лексема
                this.dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = "TokenValue",
                    HeaderText = "Лексема",
                    AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                });

                // Номер строки
                this.dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = "LineNumber",
                    HeaderText = "Номер строки",
                    Width = 50
                });

                // Позиция в строке
                this.dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = "Position",
                    HeaderText = "Позиция",
                    Width = 110
                });
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Перебираем все вкладки в TabControl
            for (int i = tabControl1.TabCount - 1; i >= 0; i--)
            {
                TabPage tabPage = tabControl1.TabPages[i];
                SplitContainer splitContainer = tabPage.Controls[0] as SplitContainer;
                RichTextBox richTextBox1 = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                // Проверяем, есть ли изменения
                if (richTextBox1.Modified)
                {
                    // Спрашиваем пользователя, хочет ли он сохранить изменения
                    DialogResult result = MessageBox.Show("Сохранить изменения в " + tabPage.Text + "?",
                                                          "Предупреждение", MessageBoxButtons.YesNoCancel,
                                                          MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Сохранение файла
                        try
                        {
                            сохранитьToolStripMenuItem_Click(sender, e);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true; // Отменяем закрытие при ошибке
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true; // Отменяем закрытие формы
                        return;
                    }
                }
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTab(null, "Новый документ", "");
        }

        private void OpenFileInNewTab(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            string fileName = Path.GetFileName(filePath);

            CreateNewTab(filePath, fileName, fileContent);
        }

        private void CreateNewTab(string filePath, string tabTitle, string fileContent)
        {
            // Создаём новую вкладку
            TabPage newTab = new TabPage(tabTitle)
            {
                Padding = new Padding(3),
                UseVisualStyleBackColor = true,
                BackColor = Color.FloralWhite
            };

            // Создаём SplitContainer для размещения двух RichTextBox
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal, // Вертикальное деление
                SplitterDistance = 54, // Как в дизайнере
                Name = "splitContainer"
            };

            // Создаём первый RichTextBox (верхний)
            RichTextBox richTextBox1 = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FloralWhite,
                Text = fileContent,
                Tag = filePath, // Если файл новый, Tag = null
                Name = "richTextBox1"
            };

            // Нижний DataGridView (таблица ошибок)
            DataGridView dataGridView1 = new DataGridView
            {
                BackgroundColor = System.Drawing.Color.AntiqueWhite,
                ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point(0, 0),
                Name = "dataGridView1",
                RowHeadersWidth = 51,
                Height = 24,
                Size = new System.Drawing.Size(756, 173),
                TabIndex = 0
            };

            // Код лексемы
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "TokenCode",
                HeaderText = "Код лексемы",
                Width = 100
            });

            // Тип лексемы (ключевое слово, идентификатор, оператор и т. д.)
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "TokenType",
                HeaderText = "Тип лексемы",
                Width = 150
            });

            // Сама лексема
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "TokenValue",
                HeaderText = "Лексема",
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            });

            // Номер строки
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "LineNumber",
                HeaderText = "Номер строки",
                Width = 80
            });

            // Позиция в строке
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "Position",
                HeaderText = "Позиция",
                Width = 80
            });

            // Добавляем элементы в SplitContainer
            splitContainer.Panel1.Controls.Add(richTextBox1);
            splitContainer.Panel2.Controls.Add(dataGridView1);

            // Добавляем SplitContainer во вкладку
            newTab.Controls.Add(splitContainer);
            tabControl1.TabPages.Add(newTab);
            tabControl1.SelectedTab = newTab;
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog.Title = "Открыть файл";
            openFileDialog.Multiselect = true; // Разрешаем выбирать несколько файлов

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    OpenFileInNewTab(filePath);
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox
                    string filePath = richTextBox.Tag as string;

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        try
                        {
                            File.WriteAllText(filePath, richTextBox.Text);
                            richTextBox.Modified = false; // Сброс флага изменения
                            MessageBox.Show("Файл сохранён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        сохранитьКакToolStripMenuItem_Click(sender, e);
                    }
                }
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить файл как";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем SplitContainer из выбранной вкладки
                    SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                    // Проверяем, что SplitContainer не равен null
                    if (splitContainer != null)
                    {
                        RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                        try
                        {
                            File.WriteAllText(saveFileDialog.FileName, richTextBox.Text);
                            richTextBox.Tag = saveFileDialog.FileName; // Запоминаем новый путь
                            tabControl1.SelectedTab.Text = Path.GetFileName(saveFileDialog.FileName); // Обновляем заголовок вкладки
                            richTextBox.Modified = false; // Сброс флага изменения
                            MessageBox.Show("Файл успешно сохранён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Перебираем все вкладки в TabControl
            for (int i = tabControl1.TabCount - 1; i >= 0; i--)
            {
                TabPage tabPage = tabControl1.TabPages[i];
                SplitContainer splitContainer = tabPage.Controls[0] as SplitContainer;
                RichTextBox richTextBox1 = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                // Проверяем, есть ли изменения
                if (richTextBox1.Modified)
                {
                    // Спрашиваем пользователя, хочет ли он сохранить изменения
                    DialogResult result = MessageBox.Show("Сохранить изменения в " + tabPage.Text + "?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Сохранение файла
                        try
                        {
                            сохранитьToolStripMenuItem_Click(sender, e);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Выход из метода при ошибке
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        // Если пользователь выбрал "Отмена", выходим из метода
                        return; // Прерываем выход
                    }
                }

                // Закрываем вкладку после проверки изменений
                tabControl1.TabPages.Remove(tabPage);
            }

            // Закрываем приложение
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            создатьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            открытьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem_Click(sender, e);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;
                RichTextBox richTextBox1 = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                if (richTextBox1.CanUndo)
                {
                    richTextBox1.Undo();
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            отменитьToolStripMenuItem_Click(sender, e);
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;
                RichTextBox richTextBox1 = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                if (richTextBox1.CanRedo)
                {
                    richTextBox1.Redo(); // Повторяем последнее действие
                }
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            повторитьToolStripMenuItem_Click(sender, e);
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                    // Проверяем, что richTextBox не равен null
                    if (richTextBox != null && richTextBox.SelectionLength > 0)
                    {
                        richTextBox.Cut(); // Вырезаем выделенный текст
                    }
                }
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            вырезатьToolStripMenuItem_Click(sender, e);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                    // Проверяем, что richTextBox не равен null и есть выделенный текст
                    if (richTextBox != null && richTextBox.SelectionLength > 0)
                    {
                        richTextBox.Copy(); // Копируем выделенный текст в буфер обмена
                    }
                }
            }
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            копироватьToolStripMenuItem_Click(sender, e);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                    // Проверяем, что richTextBox не равен null
                    if (richTextBox != null)
                    {
                        richTextBox.Paste(); // Вставляем текст из буфера обмена
                    }
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            вставитьToolStripMenuItem_Click(sender, e);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                    // Проверяем, что richTextBox не равен null
                    if (richTextBox != null)
                    {
                        // Удаляем выделенный текст
                        richTextBox.SelectedText = string.Empty;
                    }
                }
            }
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                // Получаем SplitContainer из выбранной вкладки
                SplitContainer splitContainer = tabControl1.SelectedTab.Controls[0] as SplitContainer;

                // Проверяем, что SplitContainer не равен null
                if (splitContainer != null)
                {
                    RichTextBox richTextBox = splitContainer.Panel1.Controls[0] as RichTextBox; // Верхний RichTextBox

                    // Проверяем, что richTextBox не равен null
                    if (richTextBox != null)
                    {
                        // Выделяем весь текст в RichTextBox
                        richTextBox.SelectAll();
                    }
                }
            }
        }
        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Укажите путь к вашему файлу справки
            string helpFilePath = "C:\\Users\\vfvjx\\source\\repos\\Lab1\\Lab1\\Справка.html";

            try
            {
                System.Diagnostics.Process.Start(helpFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл справки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string programInfo = "Текстовый Редактор\n" +
                                 "Версия 1.0\n" +
                                 "Автор: V.Kachigina\n" +
                                 "© 2025 Все права защищены.\n" +
                                 "Описание: Это простой текстовый редактор с поддержкой работы с файлами и базовыми функциями редактирования.";

            MessageBox.Show(programInfo, "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            вызовСправкиToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            оПрограммеToolStripMenuItem_Click(sender, e);
        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Получаем активную вкладку
            var activeTab = tabControl1.SelectedTab;

            // Проверяем, что вкладка содержит SplitContainer
            var splitContainer = activeTab.Controls.OfType<SplitContainer>().FirstOrDefault();
            if (splitContainer == null)
            {
                MessageBox.Show("Не удалось найти SplitContainer на вкладке.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Находим RichTextBox и DataGridView в соответствующих панелях SplitContainer
            var richTextBox = splitContainer.Panel1.Controls.OfType<RichTextBox>().FirstOrDefault();
            var dataGridView = splitContainer.Panel2.Controls.OfType<DataGridView>().FirstOrDefault();

            // Проверяем, что элементы существуют
            if (richTextBox != null && dataGridView != null)
            {
                string text = richTextBox.Text;

                // Создаём экземпляр сканера
                Scanner scanner = new Scanner();

                // Очищаем DataGridView перед новым анализом
                dataGridView.Rows.Clear();

                // Анализируем текст
                scanner.Analyze(text, dataGridView, richTextBox);
            }
            else
            {
                MessageBox.Show("Не удалось найти необходимые элементы на вкладке.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            пускToolStripMenuItem_Click(sender, e);
        }

    }
}
