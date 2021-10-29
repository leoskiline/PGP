
let listarAutores = {

    init: () => {
        document.getElementById("autorId").innerHTML = "<option>Carregando...</option>";
        listarAutores.obterAutores();
    },

    obterAutores: () => {
        HTTPClient.get("/Livro/ObterAutores")
            .then(result => {
                return result.json();
            })
            .then(dados => {
                listarAutores.preencherAutores(dados);
            })
            .catch(() => {
                alert("Não foi possível obter Autores.");
            })

    },

    preencherAutores: (dados) => {


        let bodyResultado = document.getElementById("autorId");

        let linhas = "";
        dados.forEach(item => {

            linhas += `<option value="${item.id}">${item.nome}</option>`
        });

        bodyResultado.innerHTML = linhas;

    },


}

document.addEventListener("DOMContentLoaded", () => {

    listarAutores.init();

});