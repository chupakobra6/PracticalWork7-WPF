using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Xml.Linq;
using Laboratory_work_No._5.overlordDataSetTableAdapters;
using Laboratory_work_No._5.Properties;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using PracticalWork4;
using static System.Net.Mime.MediaTypeNames;
using static Laboratory_work_No._5.ImportModel;
using static Laboratory_work_No._5.overlordDataSet;

namespace Laboratory_work_No._5
{
    /// <summary>
    /// Логика взаимодействия для GodsWindow.xaml
    /// </summary>
    public partial class GodsWindow : Window
    {
        overlordDataSet overlordDataSet = new overlordDataSet();

        actionTableAdapter actionAdapter = new actionTableAdapter(); // 3.14здец
        available_actionsTableAdapter availableActionsAdapter = new available_actionsTableAdapter();
        characterTableAdapter characterAdapter = new characterTableAdapter();
        character_characteristicsTableAdapter characterCharacteristicsAdapter = new character_characteristicsTableAdapter();
        character_resourcesTableAdapter characterResourcesAdapter = new character_resourcesTableAdapter();
        itemTableAdapter itemAdapter = new itemTableAdapter();
        item_priceTableAdapter itemPriceAdapter = new item_priceTableAdapter();
        minion_characteristicsTableAdapter minionCharacteristicsAdapter = new minion_characteristicsTableAdapter();
        minionsTableAdapter minionsAdapter = new minionsTableAdapter();
        profileTableAdapter profileAdapter = new profileTableAdapter();
        resourceTableAdapter resourceAdapter = new resourceTableAdapter();
        settingTableAdapter settingAdapter = new settingTableAdapter();
        settingsTableAdapter settingsAdapter = new settingsTableAdapter();
        skillTableAdapter skillAdapter = new skillTableAdapter();
        slotTableAdapter slotAdapter = new slotTableAdapter();
        typeTableAdapter typeAdapter = new typeTableAdapter();
        checkTableAdapter checkAdapter = new checkTableAdapter();

        public GodsWindow(string profileName)
        {
            InitializeComponent();

            WelcomeTextBlock.Text += profileName;

            TablesFill();

            TreeViewMethod();
        }

        private void TreeViewMethod()
        {
            Node root = new Node { Name = "Таблицы", Tables = new ObservableCollection<Table>() };

            List<DataTable> tables = overlordDataSet.Tables.Cast<DataTable>().ToList();
            foreach (DataTable table in tables)
            {
                Table tableNode = new Table { Name = table.TableName, DataTable = table, Columns = new ObservableCollection<Column>() };
                root.Tables.Add(tableNode);

                List<DataColumn> columns = table.Columns.Cast<DataColumn>().ToList();
                foreach (DataColumn column in columns)
                {
                    Column columnNode = new Column { Name = column.ColumnName, DataColumn = column, DataTable = table };
                    tableNode.Columns.Add(columnNode);
                }
            }

            ObservableCollection<Node> roots = new ObservableCollection<Node> { root };

            TablesTreeView.ItemsSource = roots;
        }

        private void TablesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Table selectedTable)
            {
                DataTable table = selectedTable.DataTable;
                TableDataGrid.ItemsSource = (System.Collections.IEnumerable)table;
                TableDataGrid.Tag = selectedTable.DataTable;

            }
            else if (e.NewValue is Column selectedColumn)
            {
                DataTable table = selectedColumn.DataTable;
                TableDataGrid.ItemsSource = (System.Collections.IEnumerable)table;
                TableDataGrid.Tag = selectedColumn.DataTable;
            }

            AddRecordToggleButton.IsChecked = false;
            EditRecordToggleButton.IsChecked = false;
            DeleteRecordToggleButton.IsChecked = false;
        }

        List<TextBox> textBoxes = new List<TextBox>();
        List<ComboBox> comboBoxes = new List<ComboBox>();

        private void AddRecordToggleButton_Checked(object sender, RoutedEventArgs e) // тут не обязательно обращатся к таблице в датасете
        {
            if (TableDataGrid.Tag != null)
            {
                DataTable table = TableDataGrid.Tag as DataTable;
                int comboBoxCount = 0;
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        TextBox textBox = new TextBox
                        {
                            VerticalAlignment = VerticalAlignment.Top,
                            FontSize = 14,
                            ToolTip = table.Columns[i].ColumnName,
                            MaxLength = 50,
                            Margin = new Thickness(15, 15, 15, 0),
                            Text = (table.Rows.Count + 1).ToString(),
                        };

                        textBoxes.Add(textBox);
                        InputStackPanel.Children.Add(textBox);
                    }
                    else if (table.Columns[i].ColumnName.Contains("id"))
                    {
                        SpecialAddRecord(table, comboBoxCount);
                        comboBoxCount++;
                    }
                    else if (!table.Columns[i].ColumnName.Contains("id")) // для колонн без ID
                    {
                        TextBox textBox = new TextBox
                        {
                            VerticalAlignment = VerticalAlignment.Top,
                            FontSize = 14,
                            ToolTip = table.Columns[i].ColumnName,
                            MaxLength = 50,
                            Margin = new Thickness(15, 5, 15, 0),
                        };

                        textBoxes.Add(textBox);
                        InputStackPanel.Children.Add(textBox);
                    }
                }
            }
        }

        private void SpecialAddRecord(DataTable table, int i)
        {
            if (table.TableName == "available_actions") // таблицы с вторичными ключами:
            {
                List<int> adapterIndexes = new List<int>
                {
                    2, 0,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "minions")
            {
                List<int> adapterIndexes = new List<int>
                {
                    2, 7,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "clan",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "item_price")
            {
                List<int> adapterIndexes = new List<int>
                {
                    10, 5,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "character_resources")
            {
                List<int> adapterIndexes = new List<int>
                {
                    10, 2,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "item")
            {
                List<int> adapterIndexes = new List<int>
                {
                    2, 15,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "slot")
            {
                List<int> adapterIndexes = new List<int>
                {
                    2, 15, 5,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "character")
            {
                List<int> adapterIndexes = new List<int>
                {
                    9, 3, 13,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "id_character_characteristics", "name"
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
            else if (table.TableName == "settings")
            {
                List<int> adapterIndexes = new List<int>
                {
                    9, 11,
                };
                List<string> columnNames = new List<string>
                {
                    "name", "name",
                };

                AddComboBox(table, adapterIndexes[i], columnNames[i]);
            }
        }

        private void AddComboBox(DataTable table, int adapterIndex, string columnName)
        {
            ComboBox comboBox = new ComboBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                ToolTip = TablesGetData()[adapterIndex].ToString().Replace("Adapter", "") + "_" + columnName,
                Margin = new Thickness(15, 5, 15, 0),
                Tag = table,
            };

            if (columnName == "id_character_characteristics")
            {
                comboBox.ItemsSource = TablesGetData()[adapterIndex].AsEnumerable().Select(row => row.Field<int>(columnName)).ToList();
                comboBox.Tag = comboBox.ItemsSource;
            }
            else
            {
                comboBox.ItemsSource = TablesGetData()[adapterIndex].AsEnumerable().Select(row => row.Field<string>(columnName)).ToList();
                comboBox.Tag = TablesGetData()[adapterIndex].AsEnumerable().Select(row => row.Field<int>("id_" + TablesGetData()[adapterIndex].ToString().Replace("Adapter", ""))).ToList();
            }

            comboBoxes.Add(comboBox);
            InputStackPanel.Children.Add(comboBox);
        }

        private void AddRecordToggleButton_Unchecked(object sender, RoutedEventArgs e) // тут надо прям к таблице в датасете
        {
            if (TableDataGrid.Tag != null)
            {
                DataTable table = TableDataGrid.Tag as DataTable;

                DataRow newRow = table.NewRow();

                bool flag = true;
                foreach (var textbox in textBoxes)
                {
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        flag = false;
                    }
                }
                foreach (var combobox in comboBoxes)
                {
                    if (combobox.SelectedItem == null)
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    bool flag2 = true;
                    int textBoxIndex = 0;
                    int comboBoxIndex = 0;

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (i == 0 || !table.Columns[i].ColumnName.Contains("id"))
                        {
                            if (table.Columns[i].DataType.Name == "Int32")
                            {
                                if (i == 0)
                                {
                                    if (int.TryParse(textBoxes[textBoxIndex].Text, out int result))
                                    {
                                        var column = table.AsEnumerable().Select(row => row.Field<int>(table.Columns[i].ColumnName));

                                        if (AddRecordToggleButton.Tag == null)
                                        {
                                            if (!column.Contains(result))
                                            {
                                                newRow[i] = result;
                                            }
                                            else flag2 = false;
                                        }
                                    }
                                    else flag2 = false;
                                }
                                else
                                {
                                    if (int.TryParse(textBoxes[textBoxIndex].Text, out int result))
                                    {
                                        if (AddRecordToggleButton.Tag == null) newRow[i] = result;
                                        else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], result);
                                    }
                                    else flag2 = false;
                                }
                            }
                            else if (table.Columns[i].DataType.Name == "Byte")
                            {
                                if (table.TableName == "settings")
                                {
                                    if (byte.TryParse(textBoxes[textBoxIndex].Text, out byte result) && result <= 100)
                                    {
                                        if (AddRecordToggleButton.Tag == null) newRow[i] = result;
                                        else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], result);
                                    }
                                    else flag2 = false;
                                }
                                else if (table.TableName == "character_characteristics")
                                {
                                    if (byte.TryParse(textBoxes[textBoxIndex].Text, out byte result) && result <= 50)
                                    {
                                        if (AddRecordToggleButton.Tag == null) newRow[i] = result;
                                        else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], result);
                                    }
                                    else flag2 = false;
                                }
                                else if (table.TableName == "minion_characteristics")
                                {
                                    if (byte.TryParse(textBoxes[textBoxIndex].Text, out byte result) && result <= 6)
                                    {
                                        if (AddRecordToggleButton.Tag == null) newRow[i] = result;
                                        else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], result);
                                    }
                                    else flag2 = false;
                                }
                            }
                            else
                            {
                                if (AddRecordToggleButton.Tag == null) newRow[i] = textBoxes[textBoxIndex].Text;
                                else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], textBoxes[textBoxIndex].Text);
                            }
                            textBoxIndex++;
                        }
                        else if (table.Columns[i].ColumnName.Contains("id"))
                        {
                            if (AddRecordToggleButton.Tag == null) newRow[i] = (comboBoxes[comboBoxIndex].Tag as List<int>)[comboBoxes[comboBoxIndex].SelectedIndex];
                            else overlordDataSet.Tables[table.TableName].Rows[TableDataGrid.SelectedIndex].SetField(table.Columns[i], (comboBoxes[comboBoxIndex].Tag as List<int>)[comboBoxes[comboBoxIndex].SelectedIndex]);
                            comboBoxIndex++;
                        }
                    }

                    if (flag2)
                    {
                        if (AddRecordToggleButton.Tag == null)
                        {
                            overlordDataSet.Tables[table.TableName].Rows.Add(newRow);
                        }
                        TableDataGrid.ItemsSource = (System.Collections.IEnumerable)overlordDataSet.Tables[table.TableName];
                        TablesUpdate();
                        TablesFill();
                    }
                }

                textBoxes.Clear();
                comboBoxes.Clear();
                InputStackPanel.Children.Clear();
            }
        }

        private void TableDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSelectedValuesInBoxes();
        }

        private void EditRecordToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (AddRecordToggleButton.IsChecked == false)
            {
                AddRecordToggleButton.Tag = "Editing";
                AddRecordToggleButton.IsChecked = true;
                GetSelectedValuesInBoxes();
            }
        }

        private void EditRecordToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            AddRecordToggleButton.IsChecked = false;
            AddRecordToggleButton.Tag = null;
        }

        private void GetSelectedValuesInBoxes() // заполнение изменения
        {
            if (EditRecordToggleButton.IsChecked == true && TableDataGrid.SelectedIndex != -1)
            {
                DataTable table = TableDataGrid.Tag as DataTable;

                int textBoxIndex = 0;
                int comboBoxIndex = 0;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i == 0 || !table.Columns[i].ColumnName.Contains("id"))
                    {
                        textBoxes[textBoxIndex].Text = table.Rows[TableDataGrid.SelectedIndex][i].ToString();
                        textBoxIndex++;
                    }
                    else if (table.Columns[i].ColumnName.Contains("id"))
                    {
                        int index = ((List<int>)comboBoxes[comboBoxIndex].Tag).IndexOf((int)table.Rows[TableDataGrid.SelectedIndex][i]);
                        comboBoxes[comboBoxIndex].SelectedItem = comboBoxes[comboBoxIndex].Items[index];
                        comboBoxIndex++;
                    }
                }
            }
        }

        private void TableDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DeleteRecordToggleButton.IsChecked == true && TableDataGrid.SelectedIndex != -1)
            {
                overlordDataSet.Tables[(TableDataGrid.Tag as DataTable).TableName].Rows[TableDataGrid.SelectedIndex].Delete();
                TablesUpdate();
                TablesFill();
                TableDataGrid.ItemsSource = (System.Collections.IEnumerable)overlordDataSet.Tables[(TableDataGrid.Tag as DataTable).TableName];
            }
        }

        List<Button> buttons = new List<Button>();
        private void ImportTablesToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Button importButton = new Button()
            {
                Name = "ImportButton",
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                Margin = new Thickness(15, 15, 15, 0),
                Content = "Выбрать файл для импорта",
            };
            importButton.Click += ImportButton_Click;

            buttons.Add(importButton);
            SecondStackPanel.Children.Add(importButton);

            Button exportButton = new Button()
            {
                Name = "ExportButton",
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                Margin = new Thickness(15, 5, 15, 0),
                Content = "Экспортировать таблицы",

            };
            exportButton.Click += ExportButton_Click;

            buttons.Add(exportButton);
            SecondStackPanel.Children.Add(exportButton);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание диалогового окна выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OverlordTextGame\\", // Задание начальной папки
                Filter = "CSV files (*.csv)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            // обработка результата
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName == settingsFilePath)
                {
                    DataTable settingsTable = overlordDataSet.Tables["settings"];
                    var settingsList = Files.Deserialization<ImportModel.Settings>(settingsFilePath);
                    foreach (ImportModel.Settings settings in settingsList)
                    {
                        DataRow row = settingsTable.NewRow();
                        row["id_profile"] = settings.id_profile;
                        row["id_setting"] = settings.id_setting;
                        row["value"] = settings.value;
                        settingsTable.Rows.Add(row);
                    }
                }
                else if (openFileDialog.FileName == charactersListPath)
                {
                    DataTable characterTable = overlordDataSet.Tables["character"];
                    var characterList = Files.Deserialization<Character>(charactersListPath);
                    foreach (Character character in characterList)
                    {
                        DataRow row = characterTable.NewRow();
                        row["id_profile"] = character.id_profile;
                        row["name"] = character.name;
                        row["id_character_characteristics"] = character.id_character_characteristics;
                        row["id_skill"] = character.id_skill;
                        characterTable.Rows.Add(row);
                    }

                }
                else if (openFileDialog.FileName == itemsListPath)
                {
                    DataTable itemTable = overlordDataSet.Tables["item"];
                    var itemList = Files.Deserialization<Item>(itemsListPath);
                    foreach (Item item in itemList)
                    {
                        DataRow row = itemTable.NewRow();
                        row["id_character"] = item.id_character;
                        row["name"] = item.name;
                        row["description"] = item.description;
                        row["amount"] = item.amount;
                        row["id_type"] = item.id_type;
                        row["power"] = item.power;
                        itemTable.Rows.Add(row);
                    }
                }
            }
        }

        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OverlordTextGame\";
        string settingsFilePath = folderPath + "settings.csv"; // путь к файлу
        string charactersListPath = folderPath + "characters.csv";
        string itemsListPath = folderPath + "items.csv";

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OverlordTextGame"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OverlordTextGame");
            }
            if (!File.Exists(settingsFilePath))
            {
                File.Create(settingsFilePath).Close();
            }
            if (!File.Exists(charactersListPath))
            {
                File.Create(charactersListPath).Close();
            }
            if (!File.Exists(itemsListPath))
            {
                File.Create(itemsListPath).Close();
            }

            DataTable settingsTable = overlordDataSet.Tables["settings"];
            DataTable characterTable = overlordDataSet.Tables["character"];
            DataTable itemTable = overlordDataSet.Tables["item"];

            var settingsList = new List<ImportModel.Settings>();
            var itemList = new List<Item>();
            var characterList = new List<Character>();

            // Заполнение коллекций данными из DataTable
            foreach (DataRow row in settingsTable.Rows)
            {
                settingsList.Add(new ImportModel.Settings
                {
                    id_profile = row.IsNull("id_profile") ? null : (int?)row["id_profile"],
                    id_setting = row.IsNull("id_setting") ? null : (int?)row["id_setting"],
                    value = row.IsNull("value") ? null : (byte?)row["value"]
                });
            }

            foreach (DataRow row in characterTable.Rows)
            {
                characterList.Add(new Character
                {
                    id_profile = row.IsNull("id_profile") ? null : (int?)row["id_profile"],
                    name = row.IsNull("name") ? null : (string)row["name"],
                    id_character_characteristics = row.IsNull("id_character_characteristics") ? null : (int?)row["id_character_characteristics"],
                    id_skill = row.IsNull("id_skill") ? null : (int?)row["id_skill"]
                });
            }

            foreach (DataRow row in itemTable.Rows)
            {
                itemList.Add(new Item
                {
                    id_character = row.IsNull("id_character") ? null : (int?)row["id_character"],
                    name = row.IsNull("name") ? null : (string)row["name"],
                    description = row.IsNull("description") ? null : (string)row["description"],
                    amount = row.IsNull("amount") ? null : (int?)row["amount"],
                    id_type = row.IsNull("id_type") ? null : (int?)row["id_type"],
                    power = row.IsNull("power") ? null : (int?)row["power"]
                });
            }

            Files.Serialization(settingsList, settingsFilePath); // сериализация данных в файл
            Files.Serialization(characterList, charactersListPath);
            Files.Serialization(itemList, itemsListPath);

            (sender as Button).Content = "setting, characters, items";
        }

        private void ImportTablesToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            buttons.Clear();
            SecondStackPanel.Children.Clear();
        }

        List<ComboBox> comboBoxes1 = new List<ComboBox>();
        private void StoreImitationToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = new ComboBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                ToolTip = TablesGetData()[2].ToString().Replace("Adapter", "") + "_first",
                Margin = new Thickness(15, 15, 15, 0),
                ItemsSource = TablesGetData()[2].AsEnumerable().Select(row => row.Field<string>("name")).ToList(),
            };

            ComboBox comboBox1 = new ComboBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                ToolTip = TablesGetData()[2].ToString().Replace("Adapter", "") + "_second",
                Margin = new Thickness(15, 5, 15, 0),
                ItemsSource = TablesGetData()[2].AsEnumerable().Select(row => row.Field<string>("name")).ToList(),
            };

            comboBoxes1.Add(comboBox);
            SecondStackPanel.Children.Add(comboBox);
            comboBoxes1.Add(comboBox1);
            SecondStackPanel.Children.Add(comboBox1);

            Button button = new Button
            {
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                Content = "Торговать",
                Margin = new Thickness(15, 5, 15, 0),
            };
            button.Click += CharacterTradeButton_Click;

            SecondStackPanel.Children.Add(button);
        }

        private void CharacterTradeButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = comboBoxes1[0];
            ComboBox comboBox1 = comboBoxes1[1];

            if (comboBox.SelectedIndex != -1 && comboBox1.SelectedIndex != -1 && comboBox.SelectedIndex != comboBox1.SelectedIndex)
            {
                TradeWindow tradeWindow = new TradeWindow(comboBox.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                tradeWindow.ShowDialog();
            }
        }

        private void StoreImitationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxes1.Clear();
            SecondStackPanel.Children.Clear();
            TablesUpdate();
            TablesFill();
        }

        private void CheckCheckButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OverlordTextGame\check.txt";
            if (File.Exists(filePath))
                if (!(Process.GetProcessesByName("notepad").Length > 0))
                    Process.Start(filePath);
        }

        private void DeleteTablesButton_Click(object sender, RoutedEventArgs e)
        {
            if (overlordDataSet != null)
            {
                overlordDataSet.Clear();
                TablesUpdate();
            }
        }

        private void TableDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Contains("Row")) e.Column = null;
            if (e.PropertyName.Contains("FK")) e.Column = null;
            if (e.PropertyName == "Table") e.Column = null;
            if (e.PropertyName == "ItemArray") e.Column = null;
            if (e.PropertyName == "HasErrors") e.Column = null;

            if (e.Column != null)
            {
                e.Column.Header = e.PropertyName.Replace("_", " ");
            }
        }

        private void TablesFill() // (3.14здец)
        {
            actionAdapter.Fill(overlordDataSet.action);
            availableActionsAdapter.Fill(overlordDataSet.available_actions);
            characterAdapter.Fill(overlordDataSet.character);
            characterCharacteristicsAdapter.Fill(overlordDataSet.character_characteristics);
            characterResourcesAdapter.Fill(overlordDataSet.character_resources);
            itemAdapter.Fill(overlordDataSet.item);
            itemPriceAdapter.Fill(overlordDataSet.item_price);
            minionCharacteristicsAdapter.Fill(overlordDataSet.minion_characteristics);
            minionsAdapter.Fill(overlordDataSet.minions);
            profileAdapter.Fill(overlordDataSet.profile);
            resourceAdapter.Fill(overlordDataSet.resource);
            settingAdapter.Fill(overlordDataSet.setting);
            settingsAdapter.Fill(overlordDataSet.settings);
            skillAdapter.Fill(overlordDataSet.skill);
            slotAdapter.Fill(overlordDataSet.slot);
            typeAdapter.Fill(overlordDataSet.type);
            checkAdapter.Fill(overlordDataSet.check);
        }

        private void TablesUpdate() // (3.14здец)
        {
            actionAdapter.Update(overlordDataSet.action);
            availableActionsAdapter.Update(overlordDataSet.available_actions);
            characterAdapter.Update(overlordDataSet.character);
            characterCharacteristicsAdapter.Update(overlordDataSet.character_characteristics);
            characterResourcesAdapter.Update(overlordDataSet.character_resources);
            itemAdapter.Update(overlordDataSet.item);
            itemPriceAdapter.Update(overlordDataSet.item_price);
            minionCharacteristicsAdapter.Update(overlordDataSet.minion_characteristics);
            minionsAdapter.Update(overlordDataSet.minions);
            profileAdapter.Update(overlordDataSet.profile);
            resourceAdapter.Update(overlordDataSet.resource);
            settingAdapter.Update(overlordDataSet.setting);
            settingsAdapter.Update(overlordDataSet.settings);
            skillAdapter.Update(overlordDataSet.skill);
            slotAdapter.Update(overlordDataSet.slot);
            typeAdapter.Update(overlordDataSet.type);
            checkAdapter.Update(overlordDataSet.check);
        }

        private List<DataTable> TablesGetData() // (3.14здец)
        {
            List<DataTable> dataTables = new List<DataTable>
            {
                actionAdapter.GetData(),
                availableActionsAdapter.GetData(),
                characterAdapter.GetData(),
                characterCharacteristicsAdapter.GetData(),
                characterResourcesAdapter.GetData(),
                itemAdapter.GetData(),
                itemPriceAdapter.GetData(),
                minionCharacteristicsAdapter.GetData(),
                minionsAdapter.GetData(),
                profileAdapter.GetData(),
                resourceAdapter.GetData(),
                settingAdapter.GetData(),
                settingsAdapter.GetData(),
                skillAdapter.GetData(),
                slotAdapter.GetData(),
                typeAdapter.GetData(),
            };
            return dataTables;
        }

        private Timer timer;

        private void ManagerButton_Click(object sender, RoutedEventArgs e)
        {
            getdrives();
            SetProgressBarMax();
            timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        private void getdrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            long max = 0;
            long totalsize = 0;
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    max += Convert.ToInt64(drive.TotalSize);
                    totalsize += Convert.ToInt64(drive.AvailableFreeSpace);
                }
            }
            DriveBar.Maximum = max / 1000000;
            DriveBar.Value += (max /= 1000000) - (totalsize /= 1000000);
            driveValue.Text += (totalsize / 1000).ToString() + " Gb";

        }
        private void SetProgressBarMax()
        {
            var computerInfo = new ComputerInfo();
            ulong totalRamBytes = (ulong)computerInfo.TotalPhysicalMemory;
            float totalRamMb = totalRamBytes / 1024f / 1024f;
            RamBar.Maximum = (int)totalRamMb;
            Console.WriteLine("Total RAM: {0:F2} MB", totalRamMb);
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            using (PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
            {
                float ramAvailableMb = ramCounter.NextValue();

                Dispatcher.Invoke(() =>
                {
                    RamValue.Text = "RAM Available: " + ramAvailableMb.ToString("F2") + " MB";
                    RamBar.Value = Convert.ToInt32(ramAvailableMb);

                });
            }
            string query = "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    float cpuPercent = (float)Convert.ToDouble(mo["PercentProcessorTime"]);

                    Dispatcher.Invoke(() =>
                    {
                        cpuValue.Text = "Cpu Value: " + cpuPercent.ToString();
                        CpuBar.Value = cpuPercent;
                    });

                    break;
                }
            }

            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync("www.google.com");

                    if (reply.Status == IPStatus.Success)
                    {
                        float latencyMs = reply.RoundtripTime;
                        Dispatcher.Invoke(() =>
                        {
                            latencyValue.Text = "Latency: " + latencyMs.ToString("F2") + " ms";
                        });
                    }
                }
                catch
                {

                }
            }

        }
    }
}
