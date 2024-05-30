import React,{ useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";

/**
 * 
 * Item do resultado da pesquisa
 * 
 * @returns 
 */
export default function PersonGroupFoundItem({connection, ownerId, personRoomId, name, email=null, isGroup}){
    
    const [changeButtonText, setChangeButtonText] = useState("");
    const [isClicked, setIsClicked] = useState(false);

    useEffect(() => {
        setChangeButtonText(isGroup === false ? "Pedir em amizade" : "Entrar");
    }, [isGroup]);

    /**
     * parametro que avalia se o botao ja foi clicado
     */
    function handleIsClicked() {
        setIsClicked(true);
    }

    /**
     * clique no botao de pedidos
     * @returns
     */
    async function handleRequestButtonClick(){

        if (isClicked) {
            return;
        }
        if(!isGroup){
            await connection.invoke("SendRequestToUser", ownerId + "", personRoomId + "");
            setChangeButtonText("Pedido Enviado");
        }else{
            await connection.invoke("EnterGroup", ownerId + "", personRoomId + "");
            setChangeButtonText("Entrou");
        }
        
    }

    return(
        <div className="person-group-found-item-div">
            <span>{personRoomId}</span>
            <br />
            <span>{name}</span>
            {!isGroup && email !== null ?
                <>
                    <br />
                    <span>{email}</span>
                </>
                :
                <></>
            }
            <button
                className="request-button"
                style={{ backgroundColor: "#3b9ae1" }}
                onClick={() => {
                    handleRequestButtonClick();
                    if (!isClicked)
                        handleIsClicked();
                }}
            >
                {changeButtonText}
            </button>
        </div>
    );

}