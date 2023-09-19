import React,{ useEffect, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

/**
 * 
 * Item do resultado da pesquisa
 * 
 * @param {*} avatar origem do avatar
 * @param personId id do utilizador
 * @param personName nome do utilizador
 * @param isGroup booleano de grupo
 * @param isRequestSent booleano de pedido enviado
 * @returns 
 */
export default function PersonGroupFoundItem({avatar, personId, personName, isGroup, isRequestSent}){

    const [requestSent, setRequestSent] = useState(isRequestSent);
    const [changeButtonText, setChangeButtonText] = useState("");
    const [changeButtonColor, setChangeButtonColor] = useState("#3b9ae1");

    useEffect(() => {
        if(isRequestSent){
            setChangeButtonText("Cancelar pedido");
            setChangeButtonColor("#ff5f4a");
        }else{
            setChangeButtonText(isGroup === false ? "Pedir amizade" : "Entrar");
            setChangeButtonColor("#3b9ae1");
        }
    }, []);

    function handleRequestButtonClick(){
        setRequestSent(!isRequestSent);
        if(isRequestSent){
            setChangeButtonText("Cancelar pedido");
            setChangeButtonColor("#ff5f4a");
        }else{
            setChangeButtonText(isGroup === false ? "Pedir amizade" : "Entrar");
            setChangeButtonColor("#3b9ae1");
        }
    }

    return(
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + personName,
                size:"40px"
            }}/>
            <TopTextBottomText TTBTProps={{
                top:personId,
                bottom:personName
            }}/>
            <Button
            text={changeButtonText}
            color={changeButtonColor}
            onClick={handleRequestButtonClick} />
        </div>
    );

}