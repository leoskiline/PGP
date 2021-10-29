let indexAutenticar = {
    entrar: () => {
        let entrada = {
            email: document.getElementById("email").value,
            senha: document.getElementById("senha").value
        };
        if (entrada.email.length > 0 && entrada.email.indexOf("@") !== -1 && entrada.email.indexOf(".") !== -1 && entrada.senha.length > 0) {
            HTTPClient.post("/Autenticar/Entrar", entrada)
                .then(result => {
                    return result.json();
                }).then(dados => {
                    if (dados.msg != "Erro") {
                        document.getElementById("msgAut").innerHTML = `<span class="text-success">Autenticado com Sucesso!</span>`;
                        localStorage.setItem("role", dados.msg);
                        setTimeout(function () {
                            window.location.href = "/Home";
                        }, 2000);
                    }
                    else {
                        document.getElementById("msgAut").innerHTML = `<span style="color:red">Usuario nao possui cadastro no sistema.</span>`;
                    }
                }).catch(() => {
                    localStorage.clear();
                    console.log("Falha ao Autenticar");
                });
        }
        else {
            document.getElementById("msgAut").innerHTML = `<span style="color:red">Preencha os campos e-mail e senha corretamente.</span>`;
        }
    }
}
