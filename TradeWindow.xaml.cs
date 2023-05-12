using Laboratory_work_No._5.overlordDataSetTableAdapters;
using Laboratory_work_No._5.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Laboratory_work_No._5.ImportModel;

namespace Laboratory_work_No._5
{
    /// <summary>
    /// Логика взаимодействия для TradeWindow.xaml
    /// </summary>
    public partial class TradeWindow : Window
    {
        string yourName;
        string hisName;

        overlordDataSet overlordDataSet = new overlordDataSet();

        characterTableAdapter characterAdapter = new characterTableAdapter();
        character_resourcesTableAdapter characterResourcesAdapter = new character_resourcesTableAdapter();
        itemTableAdapter itemAdapter = new itemTableAdapter();
        item_priceTableAdapter itemPriceAdapter = new item_priceTableAdapter();
        resourceTableAdapter resourceAdapter = new resourceTableAdapter();
        typeTableAdapter typeAdapter = new typeTableAdapter();

        checkTableAdapter checkAdapter = new checkTableAdapter();

        int characterId;
        int characterId1;
        List<TextBlock> textBlocks = new List<TextBlock>();
        List<TextBlock> textBlocks1 = new List<TextBlock>();
        public TradeWindow(string yourName, string hisName)
        {
            InitializeComponent();

            TablesFill();

            this.yourName = yourName;
            this.hisName = hisName;

            YourNameTextBlock.Text = yourName;
            HisNameTextBlock.Text = hisName;
            TradeNameTextBlock.Text += hisName;

            int characterId = (int)characterAdapter.GetIdByName(yourName).Rows[0][0];
            int characterId1 = (int)characterAdapter.GetIdByName(hisName).Rows[0][0];
            this.characterId = characterId;
            this.characterId1 = characterId1;

            UpdateBalances(characterId, characterId1);
            UpdateItems(characterId, characterId1);
        }

        private void UpdateItems(int characterId, int characterId1)
        {
            YourItemsListBox.Items.Clear();
            HisItemsListBox.Items.Clear();
            GivenAwayListBox.Items.Clear();
            ReceivedListBox.Items.Clear();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) itemAdapter.FillByCharacterId(overlordDataSet.item, characterId);
                else if (i == 1) itemAdapter.FillByCharacterId(overlordDataSet.item, characterId1);
                foreach (DataRow itemRow in overlordDataSet.item.Rows)
                {
                    int itemId = (int)itemRow["id_item"];
                    string itemName = (string)itemRow["name"];
                    int itemAmount = (int)itemRow["amount"];

                    itemPriceAdapter.FillByItemId(overlordDataSet.item_price, itemId);
                    string itemPrice = "";

                    foreach (DataRow priceRow in overlordDataSet.item_price.Rows)
                    {
                        int priceAmount = (int)priceRow["amount"];
                        int resourceId = (int)priceRow["id_resource"];
                        DataRow[] resourceRows = resourceAdapter.GetData().Select($"id_resource = {resourceId}");
                        if (resourceRows.Length > 0)
                        {
                            string priceResourceName = ((string)resourceRows[0]["name"]);
                            itemPrice += $"{priceAmount} {priceResourceName}, ";
                        }
                    }
                    itemPrice = itemPrice.TrimEnd(new char[] { ' ', ',' });

                    TextBlock itemTextBlock = new TextBlock
                    {
                        Text = $"{itemName} ({itemAmount}) - {itemPrice}",
                        Margin = new Thickness(15, 5, 5, 5),
                        ToolTip = itemName,
                        Tag = itemId,
                    };

                    if (i == 0)
                    {
                        YourItemsListBox.Items.Add(itemTextBlock);
                    }
                    else if (i == 1)
                    {
                        HisItemsListBox.Items.Add(itemTextBlock);
                    }
                }
            }
        }

        private void UpdateBalances(int characterId, int characterId1)
        {
            YourBalanceListBox.Items.Clear();
            HisBalanceListBox.Items.Clear();

            var characterResourcesData = characterResourcesAdapter.GetDataByIdCharacter(characterId);
            var characterResourcesData1 = characterResourcesAdapter.GetDataByIdCharacter(characterId1);

            foreach (var characterResourcesRow in characterResourcesData)
            {
                var resourceRow = resourceAdapter.GetDataByIdResource(characterResourcesRow.id_resource).FirstOrDefault();

                TextBlock textblock = new TextBlock
                {
                    Text = $"{resourceRow.name}: {characterResourcesRow.amount}", // задаем текст для TextBlock
                    Margin = new Thickness(15, 5, 5, 5),
                    ToolTip = "Balance",
                    Tag = characterId,
                };
                YourBalanceListBox.Items.Add(textblock); // добавляем TextBlock в контейнер
            }
            foreach (var characterResourcesRow in characterResourcesData1)
            {
                var resourceRow = resourceAdapter.GetDataByIdResource(characterResourcesRow.id_resource).FirstOrDefault();

                TextBlock textblock = new TextBlock
                {
                    Text = $"{resourceRow.name}: {characterResourcesRow.amount}", // задаем текст для TextBlock
                    Margin = new Thickness(15, 5, 5, 5),
                    ToolTip = "Balance",
                    Tag = characterId1,
                };
                HisBalanceListBox.Items.Add(textblock); // добавляем TextBlock в контейнер
            }
        }

        private void SelectedListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedTextBlock = listBox?.SelectedItem as TextBlock;

            if (selectedTextBlock != null)
            {
                listBox.Items.Remove(selectedTextBlock);
                if (listBox == YourItemsListBox)
                {
                    GivenAwayListBox.Items.Add(selectedTextBlock);
                    string price = CalculatePrice(GivenAwayListBox.Items.Cast<TextBlock>().ToList());
                    YourPayListBox.Items.Clear();
                    YourPayListBox.Items.Add($"{price}");
                    textBlocks.Add(selectedTextBlock);
                }
                else if (listBox == HisItemsListBox)
                {
                    ReceivedListBox.Items.Add(selectedTextBlock);
                    string price = CalculatePrice(ReceivedListBox.Items.Cast<TextBlock>().ToList());
                    HisPayListBox.Items.Clear();
                    HisPayListBox.Items.Add($"{price}");
                    textBlocks1.Add(selectedTextBlock);
                }
                else if (listBox == GivenAwayListBox)
                {
                    YourItemsListBox.Items.Add(selectedTextBlock);
                    string price = CalculatePrice(GivenAwayListBox.Items.Cast<TextBlock>().ToList());
                    YourPayListBox.Items.Clear();
                    YourPayListBox.Items.Add($"{price}");
                    textBlocks.Remove(selectedTextBlock);
                }
                else if (listBox == ReceivedListBox)
                {
                    HisItemsListBox.Items.Add(selectedTextBlock);
                    string price = CalculatePrice(ReceivedListBox.Items.Cast<TextBlock>().ToList());
                    HisPayListBox.Items.Clear();
                    HisPayListBox.Items.Add($"{price}");
                    textBlocks1.Remove(selectedTextBlock);
                }
            }
        }

        private string CalculatePrice(List<TextBlock> items)
        {
            TablesFill();
            Dictionary<string, int> resources = new Dictionary<string, int>();
            foreach (TextBlock item in items)
            {
                int itemId = (int)item.Tag;
                DataRow itemRow = overlordDataSet.item.AsEnumerable().FirstOrDefault(row => (int)row["id_item"] == itemId);
                int itemAmount = (int)itemRow["amount"];

                foreach (DataRow priceRow in overlordDataSet.item_price.Select($"id_item = {itemId}"))
                {
                    int priceAmount = (int)priceRow["amount"];
                    int resourceId = (int)priceRow["id_resource"];
                    DataRow[] resourceRows = resourceAdapter.GetData().Select($"id_resource = {resourceId}");
                    if (resourceRows.Length > 0)
                    {
                        string resourceName = ((string)resourceRows[0]["name"]);
                        if (resources.ContainsKey(resourceName))
                        {
                            resources[resourceName] += priceAmount * itemAmount;
                        }
                        else
                        {
                            resources[resourceName] = priceAmount * itemAmount;
                        }
                    }
                }
            }

            string price = "";
            foreach (KeyValuePair<string, int> resource in resources)
            {
                price += $"{resource.Value} {resource.Key}, ";
            }
            price = price.TrimEnd(new char[] { ' ', ',' });

            return price;
        }

        private void UpdateCharacterResources(string price, int characterId, int characterId1)
        {
            // получаем текущее количество ресурсов персонажа
            var characterResourcesData = characterResourcesAdapter.GetDataByIdCharacter(characterId);
            var characterResources = characterResourcesData.ToDictionary(row => row.id_resource, row => row.amount);

            // получаем текущее количество ресурсов у второго персонажа
            var secondCharacterResourcesData = characterResourcesAdapter.GetDataByIdCharacter(characterId1);
            var secondCharacterResources = secondCharacterResourcesData.ToDictionary(row => row.id_resource, row => row.amount);

            bool flag = true;
            List<int> ints = new List<int>();

            string[] resources = price.Split(new char[] { ',', ' ' });

            for (int i = 0; i < resources.Length; i++)
            {
                if (int.TryParse(resources[i], out int result))
                {
                    ints.Add(i);
                }
            }

            Dictionary<string, int> items = new Dictionary<string, int>();
            // вычисляем остаток ресурсов у персонажей
            foreach (int item in ints)
            {
                string word = resources[item];
                int amount = int.Parse(word);

                string resourceName = string.Join(" ", resources.Skip(1 + item).TakeWhile(x => !int.TryParse(x, out int result)));

                var resourceData = resourceAdapter.GetDataByName(resourceName);

                int resourceId = resourceData[0].id_resource;

                if (!secondCharacterResources.ContainsKey(resourceId))
                {
                    // Ресурс не найден у персонажа
                    MessageBox.Show($"Ресурс {resourceName} не найден у персонажа.");
                    flag = false;
                    break;
                }

                if (secondCharacterResources[resourceId] < amount)
                {
                    // Недостаточно ресурсов у персонажа
                    MessageBox.Show($"Недостаточно ресурсов {resourceName} у персонажа.");
                    flag = false;
                    break;
                }

                // прибавляем у первого персонажа
                if (characterResources.ContainsKey(resourceAdapter.GetDataByName(resourceName)[0].id_resource))
                {
                    characterResources[resourceAdapter.GetDataByName(resourceName)[0].id_resource] += amount;
                }

                // отнимаем у баланса второго персонажа
                if (secondCharacterResources.ContainsKey(resourceAdapter.GetDataByName(resourceName)[0].id_resource))
                {
                    secondCharacterResources[resourceAdapter.GetDataByName(resourceName)[0].id_resource] -= amount;

                    if (!items.ContainsKey(resourceName))
                    {
                        items.Add(resourceName, amount);
                    }
                    else
                    {
                        items[resourceName] += -1 * amount;
                    }
                }
            }

            if (flag)
            {
                // обновляем количество ресурсов у обоих персонажей в базе данных
                foreach (var characterResource in characterResources)
                {
                    characterResourcesAdapter.UpdateQuery(characterResource.Value, characterId, characterResource.Key);
                }

                foreach (var secondCharacterResource in secondCharacterResources)
                {
                    characterResourcesAdapter.UpdateQuery(secondCharacterResource.Value, characterId1, secondCharacterResource.Key);
                }
                CreateCheck(items);
                ItemRoll();
            }
            UpdateBalances(this.characterId, this.characterId1);
        }

        private void ConfirmTradeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCharacterResources(CalculatePrice(GivenAwayListBox.Items.Cast<TextBlock>().ToList()), characterId, characterId1);
            UpdateCharacterResources(CalculatePrice(ReceivedListBox.Items.Cast<TextBlock>().ToList()), characterId1, characterId);
        }

        private void ItemRoll()
        {
            foreach (TextBlock textBlock in textBlocks)
            {
                var newCharacterId = characterId1;
                int itemId = (int)textBlock.Tag;
                DataRow itemRow = overlordDataSet.item.AsEnumerable().FirstOrDefault(row => (int)row["id_item"] == itemId);
                itemRow["id_character"] = newCharacterId;
            }
            foreach (TextBlock textBlock in textBlocks1)
            {
                var newCharacterId = characterId;
                int itemId = (int)textBlock.Tag;
                DataRow itemRow = overlordDataSet.item.AsEnumerable().FirstOrDefault(row => (int)row["id_item"] == itemId);
                itemRow["id_character"] = newCharacterId;
            }
            itemAdapter.Update(overlordDataSet.item);
            UpdateItems(characterId, characterId1);
        }

        private void CreateCheck(Dictionary<string, int> items)
        {
            string programName = "OverlordTextGame";
            int checkId = checkAdapter.GetData().Rows.Count + 1;

            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OverlordTextGame\check.txt";

            string content = "\n\t" + programName;
            content += $"\n\tКассовый чек №{checkId}";

            foreach (var item in items)
            {
                content += $"\n\t{item.Key} - {item.Value},";
            }

            content += $"\nИтого к оплате: {items.Values.Sum()} единиц ресурса";

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(content);
            }
            checkAdapter.InsertQuery(content);
            checkAdapter.Update(overlordDataSet.check);
            checkAdapter.Fill(overlordDataSet.check);
        }

        private void TablesFill() // (3.14здец)
        {
            characterAdapter.Fill(overlordDataSet.character);
            characterResourcesAdapter.Fill(overlordDataSet.character_resources);
            itemAdapter.Fill(overlordDataSet.item);
            itemPriceAdapter.Fill(overlordDataSet.item_price);
            resourceAdapter.Fill(overlordDataSet.resource);
            typeAdapter.Fill(overlordDataSet.type);
        }

        private void TablesUpdate() // (3.14здец)
        {
            characterAdapter.Update(overlordDataSet.character);
            characterResourcesAdapter.Update(overlordDataSet.character_resources);
            itemAdapter.Update(overlordDataSet.item);
            itemPriceAdapter.Update(overlordDataSet.item_price);
            resourceAdapter.Update(overlordDataSet.resource);
            typeAdapter.Update(overlordDataSet.type);
        }

        private List<DataTable> TablesGetData() // (3.14здец)
        {
            List<DataTable> dataTables = new List<DataTable>
            {
                characterAdapter.GetData(),
                characterResourcesAdapter.GetData(),
                itemAdapter.GetData(),
                itemPriceAdapter.GetData(),
                resourceAdapter.GetData(),
                typeAdapter.GetData(),
            };
            return dataTables;
        }
    }
}
