
let listarLivros = {

    init: () => {
        document.getElementById("livroId").innerHTML = "<option>Carregando...</option>";
        listarLivros.obterLivros();
    },

    obterLivros: () => {
        HTTPClient.get("/Reserva/ObterLivros")
            .then(result => {
                return result.json();
            })
            .then(dados => {
                listarLivros.preencherEditoras(dados);
            })
            .catch(() => {
                alert("Não foi possível obter Livros.");
            })

    },

    preencherEditoras: (dados) => {


        let bodyResultado = document.getElementById("livroId");

        let linhas = "";
        dados.forEach(item => {

            linhas += `<option value="${item.id}">${item.nome}</option>`
        });

        bodyResultado.innerHTML = linhas;

    },


}

document.addEventListener("DOMContentLoaded", () => {

    listarLivros.init();

});