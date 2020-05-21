using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ScrumPoker.CrossCutting.Templates
{
    public static class EmailTemplate
    {
        public async static Task<string> GetEmailConfirmarEmailAsync(string linkBotao, string userName)
        {
            string paragrafo1 = @"
                        <p>Olá <b>" + userName + @"</b>, sejá bem-vindo(a) :).</p>
                        <p>Clique no botão a baixo para confirmar o seu e-mail.</p>
                    ";

            string paragrafo2 = "<p>Tenha um ótimo dia.</p>";

            string labelBotao = "Confirmar";

            return await GetTemplateAsync(paragrafo1, linkBotao, labelBotao, paragrafo2);

        }

        public async static Task<string> GetEmailRestarSenhaAsync(string linkBotao, string userName)
        {
            string paragrafo1 = @"
                        <p>Olá <b>" + userName + @"</b>.</p>
                        <p>Clique no botão a baixo para ser direcionado à page de recuperação de senha.</p>
                    ";

            string paragrafo2 = "<p>Tenha um ótimo dia.</p>";

            string labelBotao = "Recuperar Senha";

            return await GetTemplateAsync(paragrafo1, linkBotao, labelBotao, paragrafo2);

        }

        private async static Task<string> GetTemplateAsync(string paragrafo1 = "", string linkBotao = "", string labelBotao = "", string paragrafo2 = "")
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string NAME = "ScrumPoker.CrossCutting.Templates.EmailTemplate.html";

            using (Stream stream = assembly.GetManifestResourceStream(NAME))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var template = await reader.ReadToEndAsync();
                    template = template.Replace("@@Paragrafo1@@", paragrafo1);
                    template = template.Replace("@@LinkBotao@@", linkBotao);
                    template = template.Replace("@@LabelBotao@@", labelBotao);
                    template = template.Replace("@@Paragrafo2@@", paragrafo2);
                    return template;
                }
            }
        }
    }
}