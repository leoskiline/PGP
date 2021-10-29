
let listarLivro = {

    init: () => {
        document.getElementById("livroId").innerHTML = "<option>Carregando...</option>";
        listarLivro.obterLivros();
    },

    obterLivros: () => {
        HTTPClient.get("/Exemplar/ObterLivros")
            .then(result => {
                return result.json();
            })
            .then(dados => {
                listarLivro.preencherLivros(dados);
            })
            .catch(() => {
                alert("Não foi possível obter os livros.");
            })

    },

    preencherLivros: (dados) => {


        let bodyResultado = document.getElementById("livroId");

        let linhas = "";
        dados.forEach(item => {
            
            linhas += `<option value="${item.id}">${item.nome}</option>`
        });

        bodyResultado.innerHTML = linhas;

    },


}

document.addEventListener("DOMContentLoaded", () => {

    listarLivro.init();

});