import React,{ useEffect, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

export default function PersonGroupFoundItem({avatar, personId, personName, isGroup, requestSent}){

    const [getRequestSent, setRequestSent] = useState(requestSent);
    const [getChangeButtonText, setChangeButtonText] = useState("");
    const [getChangeButtonColor, setChangeButtonColor] = useState("#3b9ae1");

    useEffect(() => {
        if(getRequestSent){
            setChangeButtonText("Cancelar pedido");
            setChangeButtonColor("#ff5f4a");
        }else{
            setChangeButtonText(isGroup === false ? "Pedir amizade" : "Entrar");
            setChangeButtonColor("#3b9ae1");
        }
    }, []);

    function handleRequestButtonClick(){
        setRequestSent(!getRequestSent);
        if(getRequestSent){
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
            text={getChangeButtonText}
            color={getChangeButtonColor}
            onClick={handleRequestButtonClick} />
        </div>
    );

}