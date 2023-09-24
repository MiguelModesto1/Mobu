import React from "react";
import MenuItem from "../../modular/MenuItem";

export default function OwnerOptionMenu({showMenu}){ 

    const handleClick = (endpoint) => {
        switch(endpoint){
            case "perfil" : return "";
            case "jogar" : return "";
            case "procurar" : return "";
            case "terminar" : return "";
            case "fundar" : return "";
            default : return "";
        
        }
    }
    return (
        <div className="menu-container" style={{display:showMenu}}>
                <MenuItem text="Meu perfil" onClick={handleClick} onClickPrm="perfil" />
                <MenuItem text="Jogar" onClick={handleClick} onClickPrm="jogar" />
                <MenuItem text="Procurar pessoas" onClick={handleClick} onClickPrm="procurar" />
                <MenuItem text="Terminar sessão" onClick={handleClick} onClickPrm="terminar" />
                <MenuItem text="Fundar grupo" onClick={handleClick} onClickPrm="fundar" />
                <MenuItem text="Pedidos" onClick={handleClick} onClickPrm="pedidos" />
        </div>
    );
}