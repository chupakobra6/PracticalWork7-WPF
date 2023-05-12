using CsvHelper.Configuration.Attributes;
using Laboratory_work_No._5.overlordDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Laboratory_work_No._5.ImportModel;

namespace Laboratory_work_No._5
{
    /// <summary>
    /// Логика взаимодействия для CommonProfileWindow.xaml
    /// </summary>
    public partial class CommonProfileWindow : Window
    {
        overlordDataSet overlordDataSet = new overlordDataSet();

        characterTableAdapter characterAdapter = new characterTableAdapter();
        profileTableAdapter profileAdapter = new profileTableAdapter();
        character_resourcesTableAdapter characterResourcesAdapter = new character_resourcesTableAdapter();
        available_actionsTableAdapter available_ActionsAdapter = new available_actionsTableAdapter();
        minionsTableAdapter minionsAdapter = new minionsTableAdapter();
        itemTableAdapter itemAdapter = new itemTableAdapter();
        character_characteristicsTableAdapter character_CharacteristicsAdapter = new character_characteristicsTableAdapter();
        resourceTableAdapter resourceAdapter = new resourceTableAdapter();

        string profileName;
        public CommonProfileWindow(string profileName)
        {
            InitializeComponent();
            TablesFill();
            this.profileName = profileName;
            int index = 0;
            var profiles = profileAdapter.GetData();
            foreach (var profile in profiles)
            {
                if (profile.name == profileName)
                {
                    index = profile.id_profile;
                }
            }

            var characters = characterAdapter.GetData();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].id_profile == index)
                {
                    YourCharacter.Items.Add(characters[i].name);
                }
            }
        }

        List<Button> buttons = new List<Button>();
        private void YourCharacter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in buttons)
            {
                YourStackPanel.Children.Remove(item);
            }

            int index = 0;
            string name = null;
            var characters = characterAdapter.GetData();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].name == YourCharacter.SelectedItem.ToString())
                {
                    index = characters[i].id_character;
                    name = characters[i].name;
                }
            }

            var characterResources = characterResourcesAdapter.GetData();
            for (int i = 0; i < characterResources.Count; i++)
            {
                if (characterResources[i].id_character == index)
                {
                    Button button = new Button()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        FontSize = 14,
                        Content = "Добавить денег",
                        Tag = characterResources,
                        Height = 35,
                        Width = 150,
                        Margin = new Thickness(15, 15, 15, 0)
                    };
                    button.Click += Button_Click;

                    YourStackPanel.Children.Add(button);
                    buttons.Add(button);
                    break;
                }
            }

            CharacterInfo characterInfo = GetCharacterInfo(name);
            TextBlock.Text = "Имя персонажа: " + characterInfo.Name;
            TextBlock1.Text = "Макс. здоровье: " + characterInfo.Characteristics.MaxHealth;
            TextBlock2.Text = "Макс. мана: " + characterInfo.Characteristics.MaxMana;
            TextBlock3.Text = "Макс. размер свиты: " + characterInfo.Characteristics.MaxSuitSize;
            TextBlock4.Text = "Сила: " + characterInfo.Characteristics.Power;
            TextBlock5.Text = "Доступные действия: ";
            foreach (var item in characterInfo.AvailableActions)
            {
                TextBlock5.Text += item.Name + "; ";
            }
            TextBlock6.Text = "Предметы: ";
            foreach (var item in characterInfo.Items)
            {
                TextBlock6.Text += item.Name + ", сила: " + item.Power + ", кол-во: " + item.Amount + "; ";
            }
            TextBlock7.Text = "Ресурсы: ";
            foreach (var item in characterInfo.Resources)
            {
                TextBlock7.Text += item.Name + ", кол-во: " + item.Amount + "; ";
            }
        }

        public CharacterInfo GetCharacterInfo(string characterName)
        {
            var characterRow = overlordDataSet.character.FirstOrDefault(c => string.Equals(c.name, characterName, StringComparison.OrdinalIgnoreCase));
            if (characterRow == null) return null;

            // получаем характеристики персонажа
            var characteristicsRow = overlordDataSet.character_characteristics.FirstOrDefault(cc => cc.id_character_characteristics == characterRow.id_character_characteristics);

            // получаем доступные действия персонажа
            var availableActionsRows = overlordDataSet.available_actions.Where(aa => aa.id_character == characterRow.id_character)
                .Join(overlordDataSet.action, aa => aa.id_action, a => a.id_action, (aa, a) => new { a.name })
                .ToList();

            // получаем предметы персонажа
            var itemsRows = overlordDataSet.item.Where(i => i.id_character == characterRow.id_character);

            var resourcesRows = overlordDataSet.character_resources.Where(aa => aa.id_character == characterRow.id_character)
                .Join(overlordDataSet.resource,
                    aa => aa.id_resource,
                    r => r.id_resource,
                    (aa, r) => new { r.name, aa.amount })
                .ToList();


            return new CharacterInfo
            {
                Name = characterRow.name,
                Characteristics = new Characteristics
                {
                    MaxHealth = characteristicsRow?.maximum_health ?? 0,
                    MaxMana = characteristicsRow?.maximum_mana ?? 0,
                    MaxSuitSize = characteristicsRow?.maximum_suite_size ?? 0,
                    Power = characteristicsRow?.character_power ?? 0,
                },
                AvailableActions = availableActionsRows.Select(aa => new ActionInfo
                {
                    Name = aa.name,
                }).ToList(),
                Items = itemsRows.Select(i => new ItemInfo
                {
                    Name = i.name,
                    Amount = i.amount,
                    Power = i.power
                }).ToList(),
                Resources = resourcesRows.Select(r => new ResourceInfo
                {
                    Name = r.name,
                    Amount = r.amount,
                }).ToList(),
            };
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            DataRow[] rows = characterResourcesAdapter.GetData().Select();
            DataRow row = rows[new Random().Next(rows.Length)];

            int currentAmount = (int)row["amount"];
            row["amount"] = currentAmount + 100;

            characterResourcesAdapter.Update(row);
            TablesUpdate();
            TablesFill();
            YourCharacter_SelectionChanged(null, null);
        }

        private void TablesFill()
        {
            characterAdapter.Fill(overlordDataSet.character);
            profileAdapter.Fill(overlordDataSet.profile);
            resourceAdapter.Fill(overlordDataSet.resource);
            character_CharacteristicsAdapter.Fill(overlordDataSet.character_characteristics);
            characterResourcesAdapter.Fill(overlordDataSet.character_resources);
            available_ActionsAdapter.Fill(overlordDataSet.available_actions);
            minionsAdapter.Fill(overlordDataSet.minions);
            itemAdapter.Fill(overlordDataSet.item);
        }

        private void TablesUpdate()
        {
            characterAdapter.Update(overlordDataSet.character);
            profileAdapter.Update(overlordDataSet.profile);
            resourceAdapter.Update(overlordDataSet.resource);
            character_CharacteristicsAdapter.Update(overlordDataSet.character_characteristics);
            characterResourcesAdapter.Update(overlordDataSet.character_resources);
            available_ActionsAdapter.Update(overlordDataSet.available_actions);
            minionsAdapter.Update(overlordDataSet.minions);
            itemAdapter.Update(overlordDataSet.item);
        }
    }
}
