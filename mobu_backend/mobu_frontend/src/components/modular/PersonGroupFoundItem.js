import React,{ useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

/**
 * 
 * Item do resultado da pesquisa
 * 
 * @param personId id do utilizador
 * @param personName nome do utilizador
 * @param isGroup booleano de grupo
 * @param isRequestSent booleano de pedido enviado
 * @returns 
 */
export default function PersonGroupFoundItem({connection, owner, personId, name, isGroup}){

    const [changeButtonText, setChangeButtonText] = useState("");
    const [changeButtonColor, setChangeButtonColor] = useState("#3b9ae1");

    useEffect(() => {
        setChangeButtonText(isGroup === false ? "Pedir amizade" : "Entrar");
        setChangeButtonColor("#3b9ae1");
    }, [isGroup]);

    function handleRequestButtonClick(){

        if(!isGroup){
            connection.invoke("SendRequestToUser", owner + "", personId + "");
            setChangeButtonText("Pedido Enviado")
        }else{
            connection.invoke("EnterGroup", owner + "", personId + "");
            setChangeButtonText("Entrou")
        }
        
    }

    return(
        <div className="person-group-found-item-div">
            <TopTextBottomText TTBTProps={{
                top:personId,
                bottom:name
            }}/>
            <Button
            text={changeButtonText}
            color={changeButtonColor}
            onClick={handleRequestButtonClick} />
        </div>
    );

}