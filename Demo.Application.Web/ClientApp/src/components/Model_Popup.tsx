
    import React, { useState } from "react";
    import Modal from "react-modal";
import { AddEmployee } from "./AddEmployee";
import '../Model.css';

const ModalComponent = (props:any) => {

        const [isOpen, setIsOpen] = useState(false);

    function openModal() {
            if (isOpen) {
               props.RenderEmployees();
            }
            setIsOpen(!isOpen);
            
           // handleClick();
        }

        return (
            <div>
                <button onClick={openModal}>Display modal</button>

                <Modal
                    className="custom_modal"
                    overlayClassName="custom_overlay"
                    onRequestClose={openModal}
                    contentLabel="Tiny nomadic modal popover"
                    isOpen={isOpen}>
                    <h1>hello i am model</h1>
                    <AddEmployee Model={openModal} startDateIndex={0} history={props.history} location={props.locations} match={props.match} requestEmployees={props.requestEmployees} ></AddEmployee>
                     <button onClick={openModal}>Close </button>
                </Modal>
            </div>
        )

    }

    export default ModalComponent;

