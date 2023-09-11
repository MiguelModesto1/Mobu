import React,{ useState } from "react";
import Input from "../../modular/Input";
import ClickableIcon from "../../modular/ClickableIcon";

export default function SearchBar(){

    function handleChange(){
        alert("Implementar procura de pessoas na BD"); 
    }

    return(
        <div className="search-bar-div">
            <Input 
            input={{
                type:"text",
                title:"",
                value:"",
                placeholder:"Procurar pessoas ou grupos (nome, ID ou email)"
            }}
            onChange={handleChange}
            fromParent="search-bar"/>
            <ClickableIcon 
            CIProps={{
                size:"38px",
                fill:"none",
                path:{
                    d:"M33.25 33.25L23.7502 23.75M26.9167 15.8333C26.9167 21.9545 21.9545 26.9167 15.8333 26.9167C9.71218 26.9167 4.75 21.9545 4.75 15.8333C4.75 9.71218 9.71218 4.75 15.8333 4.75C21.9545 4.75 26.9167 9.71218 26.9167 15.8333Z",
                    stroke:"black",
                    strokeWidth:"2",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={() => {alert("Implementar onclick de SearchBar search icon");}}/>
        </div>
    );

}