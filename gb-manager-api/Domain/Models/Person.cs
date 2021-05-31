﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace gb_manager.Domain.Models
{
    public class Person
    {
        public int? Id { get; set; }
        public Guid? RecordId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public virtual string _Gender
        {
            get
            {
                var profileValue = "Não informado";
                
                if (Gender == null)
                    return profileValue;

                switch (Gender.ToUpper())
                {
                    case "M":
                        profileValue = "Masculino";
                        break;
                    case "F":
                        profileValue = "Feminino";
                        break;
                    case "O":
                        profileValue = "Outro";
                        break;
                    default:
                        break;
                }
                return profileValue;
            }
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        // Endereço
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string FederativeUnit { get; set; }
        public string Complement { get; set; }

        public string Password { get; set; }
        public bool Verified { get; set; }
        public string Profile { get; set; }
        public virtual string _Profile { get
            {
                var profileValue = "Desconhecido";
                switch (Profile.ToUpper())
                {
                    case "C":
                        profileValue = "Cliente";
                        break;
                    case "P":
                        profileValue = "Professor";
                        break;
                    case "A":
                        profileValue = "Administrador";
                        break;
                    default:
                        break;
                }
                return profileValue;
            }
        }
        public bool Active { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual IEnumerable<ContractPerson> Contracts { get; set; }

        public bool IsPasswordValid(string password)
        {
            return Utils.CryptoUtil.Equals(Password, password);
        }

        public string GeneratePassword(string password)
        {
            return Utils.CryptoUtil.CriarHash(password);
        }

        public void AddContract(Person person, Contract contract)
        {
            Contracts = Contracts.Concat(new List<ContractPerson>
            {
                new ContractPerson
                {
                    Person = person,
                    Contract = contract
                }
            });
        }

    }
}