using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Sincronizador_Executaveis
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ComboBoxItem> cbItens { get; set; }
        public ComboBoxItem selectedItem { get; set; }
        private Configuracao configuracao;
        private bool handle = false;

        public MainWindow()
        {
            cbItens = new ObservableCollection<ComboBoxItem>();
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var json = File.ReadAllText("config.json");
            configuracao = JsonConvert.DeserializeObject<Configuracao>(json);
            CarregarFamiliaSistemas();
        }

        private void CarregarFamiliaSistemas()
        {
            var cbItem = new ComboBoxItem { Content = "< -- Selecione a família -- >" };
            cbItens.Add(cbItem);
            selectedItem = cbItem;
            foreach (var item in configuracao.FamiliasSistema)
            {
                cbItens.Add(new ComboBoxItem { Content = item.Familia });
            }
        }

        private void cbFamilia_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) Handle();
            handle = true;
        }

        private void cbFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            Handle();
        }

        private void Handle()
        {
            if (cbFamilia.SelectedItem == null)
            {
                return;
            }
            if (cbFamilia.SelectedIndex == 0)
            {
                return;
            }
            var _familiaSistemas = cbFamilia.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            CriarAbasFamiliaSistemas(_familiaSistemas);
        }

        private void CriarAbasFamiliaSistemas(string familiaSistemas)
        {
            var sistema = configuracao.FamiliasSistema.First(q => q.Familia == familiaSistemas);
            var caminhoFamilia = configuracao.EnderecoBase + sistema.Caminho;
            var sistemas = Directory.GetDirectories(caminhoFamilia);
            foreach (var item in sistemas)
            {
                var _nome = item.ToString().Split('\\');
                var tabItem = new TabItem();
                tabItem.Header = _nome.Last();
                var versoes = Directory.GetDirectories(item.ToString());
                var conteudoAba = new StackPanel();
                foreach (var versao in versoes)
                {
                    var itemVersao = new CheckBox();
                    var nomeVersao = versao.ToString().Split('\\');
                    itemVersao.Content = nomeVersao.Last();
                    conteudoAba.Children.Add(itemVersao);
                }
                tabItem.Content = conteudoAba;
                tcSistemas.Items.Add(tabItem);
            }
        }
    }
}
