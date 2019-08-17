using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sincronizador_Executaveis
{
    class Configuracao
    {
        public string EnderecoBase { get; set; }
        public IList<FamiliaSistema> FamiliasSistema { get; set; }
    }

    public class FamiliaSistema
    {
        public string Familia { get; set; }
        public string Caminho { get; set; }
    }
}
