import React,{ useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";

/**
 * 
 * Item do resultado da pesquisa
 * 
 * @returns 
 */
export default function PersonGroupFoundItem({connection, ownerId, personRoomId, name, avatar, email=null, isGroup}){
    
    const [changeButtonText, setChangeButtonText] = useState("");
    const [isClicked, setIsClicked] = useState(false);

    useEffect(() => {

        if (!isGroup) {
            connection.on("ReceiveRequest", (user, fromUsername) => {
                if (personRoomId + "" === user) {
                    setChangeButtonText("Pedido Enviado");
                }
            });
        }
        else {
            connection.on("ReceiveEntry", (group, message) => {
                if (personRoomId + "" === group) {
                    setChangeButtonText("Entrou");
                }
            });
        }
        

        
    },[])

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

        if(!isGroup){
            await connection.invoke("SendRequestToUser", ownerId + "", personRoomId + "");
        }else{
            await connection.invoke("EnterGroup", ownerId + "", personRoomId + "");
        }
        
    }

    return(
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                size: "40px",
                src: avatar,
                alt: "avatar de " + personRoomId
            }} />
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
                disabled={isClicked}
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