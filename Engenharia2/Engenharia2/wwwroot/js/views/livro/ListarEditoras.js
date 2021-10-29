
let listarEditoras = {

    init: () => {
        document.getElementById("editoraId").innerHTML = "<option>Carregando...</option>";
        listarEditoras.obterEditoras();
    },

    obterEditoras: () => {
        HTTPClient.get("/Livro/ObterEditoras")
            .then(result => {
                return result.json();
            })
            .then(dados => {
                listarEditoras.preencherEditoras(dados);
            })
            .catch(() => {
                alert("Não foi possível obter Editoras.");
            })

    },

    preencherEditoras: (dados) => {


        let bodyResultado = document.getElementById("editoraId");

        let linhas = "";
        dados.forEach(item => {

            linhas += `<option value="${item.id}">${item.nome}</option>`
        });

        bodyResultado.innerHTML = linhas;

    },


}

document.addEventListener("DOMContentLoaded", () => {

    listarEditoras.init();

});