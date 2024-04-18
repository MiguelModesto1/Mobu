import React,{useEffect, useRef, useState} from 'react';
import RequestItem from '../../modular/RequestItem';
import {HubConnectionBuilder as signalR} from "@microsoft/signalr";

/**
 * Mosaico para escolher oponente de jogo
 * @returns 
 */
export default function FriendshipReqTile(){

    const requests = useRef([]);

    const queryParams = new URLSearchParams(window.location.search);

    const connectionId = queryParams.get("connectionId");

    const connection = new signalR.HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_HUB_URL + `/RequestHub?connection=${connectionId}`)
    .configureLogging(signalR.LogLevel.Information)
    .build();

    const id = parseInt(queryParams.get("id"));

    
    connection.on("ReceiveRequest", function(fromUser, fromUsername){
        requests.current.push([
            parseInt(fromUser),
            fromUsername
        ])
    });

    /*useEffect(() =>{
       
        // GET
        var options={
            method:"GET",
            redirect:"follow"
        }

        fetch(process.env.REACT_APP_API_URL + "/requests?id=" + id, options)
        .then(response => {
            if(response.status !== 404){
                return response.json();
            }
        })
        .then(data => {

            var reqData = [];

            var reqListLen = data.reqListLen;

            for(let i=0; i < reqListLen; i++){
                reqData[i] = data.requests[i]
            }

            requests.current = reqData;
        });
    }, [id]);*/

    function handleClick(isAccept, requester)
    {

        var aux = [];

        if(isAccept){
            connection.invoke("SendRequestReply", requester[0] + "", id + "", true);
        }else{
            connection.invoke("SendRequestReply", requester[0] + "", id + "", false);
        }

        for(let i=0; i < requests.current.length; i++){
            if(requests.current[i] !== requester){
                aux[i] = requests.current[i]
            }
        }

        requests.current = aux;
    }

    /*const mapItems = requests.current.map((item) =>{

        return(
            <RequestItem
            key={item[0]}
            PIProps={{
                info: item,
                text:{
                    top: item[1],
                    bottom: item[0]
                }
            }}
            onClick={handleClick}/>
        );
    })*/

    return(
        <div className='friendship-req-tile'>
            {/*mapItems*/}
        </div>
    );

}