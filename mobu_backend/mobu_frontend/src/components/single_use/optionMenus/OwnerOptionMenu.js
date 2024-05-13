import React from "react";
import MenuItem from "../../modular/MenuItem";

export default function OwnerOptionMenu({owner, showMenu, logoutCallback}){ 

    const handleClick = (endpoint) => {
        switch (endpoint) {
            case "perfil":
                window.location.assign("/person-profile&id=" + owner);
                break;
            case "procurar":
                window.location.assign("/search" + owner);
                break;
            case "terminar":
                logoutCallback();
                window.location.assign("/");
                break;
            default:
                window.location.assign("/group-foundation");
                break;
        
        }
    }
    return (
        <div className="menu-container" style={{display:showMenu}}>
            <MenuItem text="Meu perfil" onClick={handleClick} onClickPrm="perfil" />
            <MenuItem text="Procurar pessoas" onClick={handleClick} onClickPrm="procurar" />
            <MenuItem text="Fundar grupo" onClick={handleClick} onClickPrm="fundar" />
            <MenuItem text="Terminar sessão" onClick={handleClick} onClickPrm="terminar" />
        </div>
    );
}