let indexEmprestimo = {

    carregar: () => {

        HTTPClient.get("/Emprestimo/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                var dataEmp, dataPrevistaDevol;
                dados.forEach(item => {
                    dataEmp = new Date(`${item.dataEmp}`).toLocaleDateString().substr(0, 10);
                    dataPrevistaDevol = new Date(`${item.dataPrevistaDevol}`).toLocaleDateString().substr(0, 10);
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.leitor.cpf}</td>
                        <td><select class="form-control">${indexEmprestimo.carregarLivros(item.exemplar)}</select></td>
                        <td>${dataEmp}</td>
                        <td>${dataPrevistaDevol}</td>
                        <td>${item.situacao}</td>
                        <td onclick='indexEmprestimo.devolver(${item.id})'>X</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    carregarLivros: (exemplar) => {
        var option = "";
        exemplar.forEach(item => {
            option += `<option value="${item.id}">${item.livro.nome}</option>`;
        });
        return option;
    },

    montarLivros: (livro) => {
        var select = document.querySelector("[data-select]");
        livro.forEach(item => {
            for (let i = 0; i < select.options.length; i++) {
                select.options[i].removeAttribute("selected", "selected");
            }
        });
        livro.forEach(item => {
            for (let i = 0; i < select.options.length; i++) {
                if (select.options[i].value == item.id)
                    select.options[i].setAttribute("selected", "selected");
            }
        });
    },

    pesquisar: () => {
        var cpf = document.getElementById("pesquisaLeitor").value;
        HTTPClient.get("/Emprestimo/Pesquisar?cpf=" + cpf)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                var dataEmp, dataPrevistaDevol;
                
                dados.forEach(item => {
                    dataEmp = new Date(`${item.dataEmp}`).toLocaleDateString().substr(0, 10);
                    dataPrevistaDevol = new Date(`${item.dataPrevistaDevol}`).toLocaleDateString().substr(0, 10);
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.leitor.cpf}</td>
                        <td><select class="form-control">${indexEmprestimo.carregarLivros(item.exemplar)}</select></td>
                        <td>${dataEmp}</td>
                        <td>${dataPrevistaDevol}</td>
                        <td>${item.situacao}</td>
                        <td onclick='indexEmprestimo.devolver(${item.id})'>X</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
                document.getElementById("pesquisaLeitor").value = "";
            }).catch(function (error) {
                console.error(error);
            });
    },

    enviar: () => {
        var select = document.getElementById("livroId")
        var livrosId = [];
        for (let i = 0; i < select.options.length; i++) {
            if (select.options[i].selected)
                livrosId.push(select.options[i].value);
        }
        let dados = {
            cpf: document.getElementById("cpf").value,
            livro: livrosId
        }
        debugger
        HTTPClient.post("/Emprestimo/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Emprestimo!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";

                indexEmprestimo.carregar();
                indexEmprestimo.limparForm();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });
    },

    devolver: (id) => {
        HTTPClient.put("/Emprestimo/Devolver?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexEmprestimo.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
    },

    limparForm: () => {
        var fdados = document.getElementById("emprestimo-fdados");
        fdados.cpf.value = "";
        fdados.livroId.options[0];
    }

}



document.addEventListener("DOMContentLoaded", () => {
    indexEmprestimo.carregar();
});