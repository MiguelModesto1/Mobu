import React from "react";
import MenuItem from "../modular/MenuItem";
import { useNavigate } from 'react-router-dom';

/**
 * 
 * Menu de acesso do dono da conta
 * 
 * @param owner - O ID do dono da conta
 * @param showMenu - Indica se o menu deve ser mostrado ou não
 * @param connection - Conexão SignalR
 * @param logoutCallback - Callback de logout
 * 
 * @returns
 */
export default function OwnerOptionMenu({owner, showMenu, connection, logoutCallback}){ 

    const navigate = useNavigate();

    const handleClick = async (option) => {
        switch (option) {
            case "perfil":
                await connection.stop();
                navigate("/person-profile?id=" + owner + "&requester=" + owner);
                break;
            case "procurar":
                await connection.stop();
                navigate("/search?id=" + owner);
                break;
            case "terminar":
                logoutCallback();
                navigate("/");
                break;
            case "fundar":
                await connection.stop();
                navigate("/group-foundation?id=" + owner);
                break;
            default:
                await connection.stop();
                navigate("/pending-requests?id=" + owner);
                break;
        
        }
    }
    return (
        <div className="menu-container" style={{ display: showMenu, position: "absolute", right:"0%" }}>
            <MenuItem text="Meu perfil" onClick={handleClick} onClickPrm="perfil" />
            <MenuItem text="Pesquisar" onClick={handleClick} onClickPrm="procurar" />
            <MenuItem text="Pedidos de amizade" onClick={handleClick} onClickPrm="pedidos" />
            <MenuItem text="Fundar grupo" onClick={handleClick} onClickPrm="fundar" />
            <MenuItem text="Terminar sessão" onClick={handleClick} onClickPrm="terminar" />
        </div>
    );
}