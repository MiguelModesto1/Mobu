import React,{ useState } from "react";
import Input from "../../modular/Input";
import ClickableIcon from "../../modular/ClickableIcon";
import PersonGroupFoundItem from "../../modular/PersonGroupFoundItem";
import {HubConnection as signalR} from "@microsoft/signalr";

/**
 * Barra de pesquisa de pessoas
 * @returns 
 */
export default function SearchPage(){

    const [query, setQuery] = useState("");
    const [items, setItems] = useState([]);

    const queryParams = new URLSearchParams(window.location.href);

    const connectionId = queryParams.get("connectionId");
    const owner = parseInt(queryParams.get("owner"));

    const connection = signalR.HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_HUB_URL + `/RequestHub?connection=${connectionId}`)
    .configureLogging(signalR.LogLevel.Information)
    .build();

    function handleChange(value){
        setQuery(value); 
    }

    async function handleIconClick(){
        let options={
            method:'GET',
            redirect:'follow'
        }

        const queryStrings = `?query=${query}`

        fetch(process.env.REACT_APP_API_URL + "/search" + queryStrings, options)
        .then((response) => {
            if (response.status === 200) {
                return response.json()
            }
        })
        .then((data) => {
            setItems(data["unknown"]);
        })
        .catch((err) => {console.error("error", err)});
    }

    const mapItems = items.map((item) => {
        if (item.length === 3) {
            return (
                <PersonGroupFoundItem
                    key={item[0]}
                    connection={connection}
                    owner={owner}
                    personId={item[0]}
                    name={item[1]}
                    isGroup={true} />);
        } else {
            return (
                <PersonGroupFoundItem
                    key={item[0]}
                    connection={connection}
                    owner={owner}
                    personId={item[0]}
                    name={item[1]}
                    isGroup={false} />);
        }
    });

    return(
    <div className="search-div">
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
            onClick={handleIconClick}/>
        </div>
        <div className="found-div">
            {mapItems}
        </div>
    </div>
        
    );

}